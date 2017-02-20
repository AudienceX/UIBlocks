using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace UIBlocks.MaterialDesignTheme
{
    public static class RippleEditor
    {
        #region ClipToBoundsProperty，附加属性,效果是否溢出边框
        public static readonly DependencyProperty ClipToBoundsProperty = DependencyProperty.RegisterAttached(
"ClipToBounds", typeof(bool), typeof(RippleEditor), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.Inherits));
        public static void SetClipToBounds(DependencyObject element, bool value)
        {
            element.SetValue(ClipToBoundsProperty, value);
        }
        public static bool GetClipToBounds(DependencyObject element)
        {
            return (bool)element.GetValue(ClipToBoundsProperty);
        }
        #endregion

        #region disable，附加属性，是否禁用波纹效果
        public static readonly DependencyProperty disableProperty = DependencyProperty.RegisterAttached(
            "disable", typeof(bool), typeof(RippleEditor), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

        public static void Setdisable(DependencyObject element, bool value)
        {
            element.SetValue(disableProperty, value);
        }

        public static bool Getdisable(DependencyObject element)
        {
            return (bool)element.GetValue(disableProperty);
        }
        #endregion

        #region FeedBackBrush，附加属性，波纹颜色。
        public static readonly DependencyProperty FeedBackBrushProperty = DependencyProperty.RegisterAttached(
            "FeedBackBrush", typeof(Brush), typeof(RippleEditor), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender));
        public static void SetFeedBackBrush(DependencyObject element, Brush value)
        {
            element.SetValue(FeedBackBrushProperty, value);
        }
        public static Brush GetFeedBackBrush(DependencyObject element)
        {
            return (Brush) element.GetValue(FeedBackBrushProperty);
        }
        #endregion

        #region OnCenter,附加属性，波纹是否在中间显示。
        public static readonly DependencyProperty OnCenterProperty = DependencyProperty.RegisterAttached(
            "OnCenter", typeof(bool), typeof(RippleEditor), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));
        public static void SetOnCenter(DependencyObject element, bool value)
        {
            element.SetValue(OnCenterProperty, value);
        }
        public static bool GetOnCenter(DependencyObject element)
        {
            return (bool) element.GetValue(OnCenterProperty);
        }
        #endregion

    }
}
