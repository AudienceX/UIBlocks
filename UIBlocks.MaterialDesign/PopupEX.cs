using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;

namespace UIBlocks.MaterialDesign
{
    /// <summary>
    /// 为系统默认的PopupBox添加以下功能
    /// 当宿主控件位置改变或窗口最大化时，重新定位弹出位置
    /// 当弹出时是窗口上唯一被激活的控件
    /// </summary>
    public class PopupEX:Popup
    {
        #region CanBeClosedByMouseLeftButtonDown,弹出内容能够被鼠标左键点击关闭
        public static readonly DependencyProperty CanBeClosedByMouseLeftButtonDownProperty = DependencyProperty.Register(
            "弹出内容能够被鼠标左键点击关闭", typeof(bool), typeof(PopupEX), new PropertyMetadata(default(bool)));
        /// <summary>
        /// 弹出内容能够被鼠标左键点击关闭
        /// </summary>
        public bool CanBeClosedByMouseLeftButtonDown
        {
            get { return (bool) GetValue(CanBeClosedByMouseLeftButtonDownProperty); }
            set { SetValue(CanBeClosedByMouseLeftButtonDownProperty, value); }
        }
        #endregion

        private Window hostwindow;
        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        static readonly IntPtr HWND_TOP = new IntPtr(0);
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        private bool? IsTopMost;
        internal enum SWP
        {
            ASYNCWINDOWPOS = 0x4000,
            DEFERERASE = 0x2000,
            DRAWFRAME = 0x0020,
            FRAMECHANGED = 0x0020,
            HIDEWINDOW = 0x0080,
            NOACTIVATE = 0x0010,
            NOCOPYBITS = 0x0100,
            NOMOVE = 0x0002,
            NOOWNERZORDER = 0x0200,
            NOREDRAW = 0x0008,
            NOREPOSITION = 0x0200,
            NOSENDCHANGING = 0x0400,
            NOSIZE = 0x0001,
            NOZORDER = 0x0004,
            SHOWWINDOW = 0x0040,
            TOPMOST = SWP.NOACTIVATE | SWP.NOOWNERZORDER | SWP.NOSIZE | SWP.NOMOVE | SWP.NOREDRAW | SWP.NOSENDCHANGING,
        }
        internal struct SIZE
        {
            public int cx;
            public int cy;
        }

        internal static int LOWORD(int i)
        {
            return (short)(i & 0xFFFF);
        }

        internal struct POINT
        {
            public int x;
            public int y;
        }

        internal struct RECT
        {
            private int _left;
            private int _right;
            private int _top;
            private int _bottom;
            public int Left {
                get { return _left; }
                set { _left = value; }
            }
            public int Right    
            {
                get { return _right; }
                set { _right = value; }
            }

            public int Top
            {
                get { return _top; }
                set { _top = value; }
            }

            public int Bottom
            {
                get { return _bottom; }
                set { _bottom = value; }
            }

            public int Width
            {
                get { return _right - _left; }
            }

            public int Height
            {
                get { return _bottom - _top; }
            }

            public POINT Position
            {
                get { return new POINT { x = _left, y = _top }; }
            }

            public SIZE Size
            {
                get { return new SIZE { cx = Width, cy = Height }; }
            }

            public void Offset(int dx, int dy)
            {
                _left += dx;
                _right += dx;
                _top += dy;
                _bottom += dy;
            }

            public static RECT Union(RECT rect1, RECT rect2)
            {
                return new RECT
                {
                    Left = Math.Min(rect1.Left, rect2.Left),
                    Top = Math.Min(rect1.Top, rect2.Top),
                    Right = Math.Max(rect1.Right, rect2.Right),
                    Bottom = Math.Max(rect1.Bottom, rect2.Bottom),
                };
            }

            public override bool Equals(object obj)
            {
                try
                {
                    var rc = (RECT)obj;
                    return rc._bottom == _bottom
                        && rc._left == _left
                        && rc._right == _right
                        && rc._top == _top;
                }
                catch (InvalidCastException)
                {
                    return false;
                }
            }


            public override int GetHashCode()
            {
                return (_left << 16 | LOWORD(_right)) ^ (_top << 16 | LOWORD(_bottom));
            }
        }

        public PopupEX()
        {
            this.Loaded += PopupEx_Loaded;
            this.Unloaded += PopupEx_Unloaded;
        }

        public void RefreshPosition()
        {
            var offset = this.HorizontalOffset;
 
            SetCurrentValue(HorizontalOffsetProperty, offset + 1);
            SetCurrentValue(HorizontalOffsetProperty, offset);
        }

        //加载初始化
        private void PopupEx_Loaded(object sender, RoutedEventArgs e)
        {
            var target = this.PlacementTarget as FrameworkElement;
            if(target==null)
                return;
            this.hostwindow=Window.GetWindow(target);
            if(hostwindow==null) return;

            this.hostwindow.LocationChanged -= this.hostwindow_SizeOrLocationChanged;
            this.hostwindow.LocationChanged += this.hostwindow_SizeOrLocationChanged;
            this.hostwindow.SizeChanged -= this.hostwindow_SizeOrLocationChanged;
            this.hostwindow.SizeChanged += this.hostwindow_SizeOrLocationChanged;
            target.SizeChanged -= this.hostwindow_SizeOrLocationChanged;
            target.SizeChanged += this.hostwindow_SizeOrLocationChanged;
            this.hostwindow.Activated -= this.hostwindow_Activated;
            this.hostwindow.Activated += this.hostwindow_Activated;
            this.hostwindow.Deactivated -= this.hostwindow_Deactivated;
            this.hostwindow.Deactivated += this.hostwindow_Deactivated;

            this.Unloaded -= this.PopupEx_Unloaded;
            this.Unloaded += this.PopupEx_Unloaded;
        }

        //当大小位置变化时重新计算弹出位置。
        private void hostwindow_SizeOrLocationChanged(object sender, EventArgs e)
        {
            RefreshPosition();
        }

        //将弹出内容设置为最顶层
        private void SetTopmostState(bool isTop)
        {
            if (this.IsTopMost.HasValue && this.IsTopMost == isTop)
            {
                return;
            }
            if(this.Child==null)
                return;

            var hwndSource = (PresentationSource.FromVisual(this.Child)) as HwndSource;
            if(hwndSource==null)
                return;
            var hwnd = hwndSource.Handle;
            RECT rect;
            if (!GetWindowRect(hwnd, out rect))
            {
                return;
            }

            var left = rect.Left;
            var top = rect.Top;
            var width = rect.Width;
            var height = rect.Height;
            if (isTop)
            {
                SetWindowPos(hwnd, HWND_TOPMOST, left, top, width, height, SWP.TOPMOST);
            }
            else
            {
                    SetWindowPos(hwnd, HWND_BOTTOM, left, top, width, height, SWP.TOPMOST);
                    SetWindowPos(hwnd, HWND_TOP, left, top, width, height, SWP.TOPMOST);
                    SetWindowPos(hwnd, HWND_NOTOPMOST, left, top, width, height, SWP.TOPMOST);
            }

            this.IsTopMost = true;
        }

        private void hostwindow_Deactivated(object sender, EventArgs e)
        {
            this.SetTopmostState(false);
        }

        private void hostwindow_Activated(object sender, EventArgs e)
        {
            this.SetTopmostState(true);
        }


        private void PopupEx_Opened(object sender, EventArgs e)
        {
            this.SetTopmostState(true);
        }

        //鼠标点击事件
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (CanBeClosedByMouseLeftButtonDown)
            {
                this.IsOpen = false;
            }
        }

        private void PopupEx_Unloaded(object sender, RoutedEventArgs e)
        {
            var target = this.PlacementTarget as FrameworkElement;
            if (target != null)
            {
                target.SizeChanged -= this.hostwindow_SizeOrLocationChanged;
            }
            if (this.hostwindow != null)
            {
                this.hostwindow.LocationChanged -= this.hostwindow_SizeOrLocationChanged;
                this.hostwindow.SizeChanged -= this.hostwindow_SizeOrLocationChanged;
                this.hostwindow.Activated -= this.hostwindow_Activated;
                this.hostwindow.Deactivated -= this.hostwindow_Deactivated;
            }
            this.Unloaded -= this.PopupEx_Unloaded;
            this.Opened -= this.PopupEx_Opened;
            this.hostwindow = null;
        }

        [SecurityCritical]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport("user32.dll", EntryPoint = "GetWindowRect", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [SecurityCritical]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport("user32.dll", EntryPoint = "SetWindowPos", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool _SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SWP uFlags);

        [SecurityCritical]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private static bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SWP uFlags)
        {
            if (!_SetWindowPos(hWnd, hWndInsertAfter, x, y, cx, cy, uFlags))
            {
                // If this fails it's never worth taking down the process.  Let the caller deal with the error if they want.
                return false;
            }
            return true;
        }
    }

}
