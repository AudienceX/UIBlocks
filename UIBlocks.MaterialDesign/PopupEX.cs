using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

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

        public PopupEX()
        {
            this.Loaded += PopupEx_Loaded;
        }

        public void RefreshPosition()
        {
            var offset = this.HorizontalOffset;
 
            SetCurrentValue(HorizontalOffsetProperty, offset + 1);
            SetCurrentValue(HorizontalOffsetProperty, offset);
        }

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

            throw new NotImplementedException();
        }

        //当大小位置变化时重新计算弹出位置。
        private void hostwindow_SizeOrLocationChanged(object sender, EventArgs e)
        {
            RefreshPosition();
        }

        //将弹出内容设置为最顶层
        private void SetTopmostState(bool isTop)
        {
            throw new NotImplementedException();
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

    }

}
