using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;

namespace UIBlocks.MaterialDesign
{
    /// <summary>
    /// 相比原有的默认Popup，多了支持在移动窗口后自动调整弹出内容的过程。
    /// </summary>
    public class PopupEx:Popup
    {
        public PopupEx()
        {
            this.Loaded += PopupEx_Loaded;
        }

        private void PopupEx_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var target = this.PlacementTarget as FrameworkElement;
            if (target != null)
                target.SizeChanged -= HostwindowSizeOrLocationChanged;
            if (hostwindow != null)
            {
                hostwindow.SizeChanged -= HostwindowSizeOrLocationChanged;
                hostwindow.LocationChanged -= HostwindowSizeOrLocationChanged;
            }
        }


        private Window hostwindow;
        private void PopupEx_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var target = this.PlacementTarget as FrameworkElement;
            if(target==null) return;
            hostwindow=Window.GetWindow(target);
            if(hostwindow==null) return;
            this.hostwindow.LocationChanged -= HostwindowSizeOrLocationChanged;
            this.hostwindow.LocationChanged += HostwindowSizeOrLocationChanged;
            this.hostwindow.SizeChanged -= HostwindowSizeOrLocationChanged;
            this.hostwindow.SizeChanged += HostwindowSizeOrLocationChanged;
            target.SizeChanged -= HostwindowSizeOrLocationChanged;
            target.SizeChanged += HostwindowSizeOrLocationChanged;
            this.Unloaded -= this.PopupEx_Unloaded;
            this.Unloaded += this.PopupEx_Unloaded;
        }

        private void HostwindowSizeOrLocationChanged(object sender, EventArgs e) => RefrershPosition();

        /// <summary>
        /// 刷新弹出内容位置，否则窗口移动后弹出内容任然不变。
        /// </summary>
        private void RefrershPosition()
        {
            var offset = this.HorizontalOffset;
            SetCurrentValue(HorizontalOffsetProperty,offset+1);
            SetCurrentValue(HorizontalOffsetProperty,offset);
        }
    }
}
