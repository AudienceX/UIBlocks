using System.Windows;

namespace UIBlocks.MaterialDesign
{
    //静态类，定义各种附加属性用来表达波纹效果的状态。
    public static class RippleEditor
    {
        // 附加属性，设置波纹效果是否从控件中心产生，如果为false则从鼠标点击位置产生。
        public static readonly DependencyProperty IsInCenterProperty =
            DependencyProperty.RegisterAttached("IsInCenter", typeof(bool), typeof(RippleEditor),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

        //附加属性，设置波纹效果是否被开启 。
        public static readonly DependencyProperty EnableProperty =
            DependencyProperty.RegisterAttached("Enable", typeof(bool), typeof(RippleEditor),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.Inherits));

        public static bool GetIsInCenter(DependencyObject obj)
        {
            return (bool) obj.GetValue(IsInCenterProperty);
        }

        public static void SetIsInCenter(DependencyObject obj, bool value)
        {
            obj.SetValue(IsInCenterProperty, value);
        }


        public static bool GetEnable(DependencyObject obj)
        {
            return (bool) obj.GetValue(EnableProperty);
        }

        public static void SetEnable(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableProperty, value);
        }
    }
}