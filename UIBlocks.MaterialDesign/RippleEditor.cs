using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace UIBlocks.MaterialDesign
{
    public static class RippleEditor
    {
        public static bool GetIsInCenter(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsInCenterProperty);
        }
        public static void SetIsInCenter(DependencyObject obj, bool value)
        {
            obj.SetValue(IsInCenterProperty, value);
        }
        // 附加属性，设置波纹效果是否从控件中心产生，如果为false则从鼠标点击位置产生
        public static readonly DependencyProperty IsInCenterProperty =
            DependencyProperty.RegisterAttached("IsInCenter", typeof(bool), typeof(RippleEditor), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));


        public static bool GetEnable(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableProperty);
        }
        public static void SetEnable(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableProperty, value);
        }
        //附加属性，设置波纹效果是否被开启 
        public static readonly DependencyProperty EnableProperty =
            DependencyProperty.RegisterAttached("Enable", typeof(bool), typeof(RippleEditor), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.Inherits));
    }
}

