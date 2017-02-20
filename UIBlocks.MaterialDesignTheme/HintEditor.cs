using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UIBlocks.MaterialDesignTheme
{
    public static class HintEditor
    {
        #region UseFloating,附加属性，是否使用浮动
        public static readonly DependencyProperty IsFloatingProperty = DependencyProperty.RegisterAttached(
            "IsFloating",
            typeof(bool),
            typeof(HintEditor),
            new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.Inherits));
        public static bool GetIsFloating(DependencyObject element)
        {
            return (bool)element.GetValue(IsFloatingProperty);
        }
        public static void SetIsFloating(DependencyObject element, bool value)
        {
            element.SetValue(IsFloatingProperty, value);
        }
        #endregion

        #region FloatingScale & FloatingOffset

        public static readonly DependencyProperty FloatingScaleProperty = DependencyProperty.RegisterAttached(
            "FloatingScale",
            typeof(double),
            typeof(HintEditor),
            new FrameworkPropertyMetadata(0.74d, FrameworkPropertyMetadataOptions.Inherits));

        public static double GetFloatingScale(DependencyObject element)
        {
            return (double)element.GetValue(FloatingScaleProperty);
        }

        public static void SetFloatingScale(DependencyObject element, double value)
        {
            element.SetValue(FloatingScaleProperty, value);
        }

        public static readonly DependencyProperty FloatingOffsetProperty = DependencyProperty.RegisterAttached(
            "FloatingOffset",
            typeof(Point),
            typeof(HintEditor),
            new FrameworkPropertyMetadata(new Point(1, -16), FrameworkPropertyMetadataOptions.Inherits));

        public static Point GetFloatingOffset(DependencyObject element)
        {
            return (Point)element.GetValue(FloatingOffsetProperty);
        }

        public static void SetFloatingOffset(DependencyObject element, Point value)
        {
            element.SetValue(FloatingOffsetProperty, value);
        }
        #endregion

        #region Hint
        public static readonly DependencyProperty HintProperty = DependencyProperty.RegisterAttached(
            "Hint",
            typeof(object),
            typeof(HintEditor),
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.Inherits));
        public static void SetHint(DependencyObject element, object value)
        {
            element.SetValue(HintProperty, value);
        }
        public static object GetHint(DependencyObject element)
        {
            return element.GetValue(HintProperty);
        }
        #endregion

        #region HintOpacity
        public static readonly DependencyProperty HintOpacityProperty = DependencyProperty.RegisterAttached(
            "HintOpacity",
            typeof(double),
            typeof(HintEditor),
            new PropertyMetadata(.56));
        public static double GetHintOpacityProperty(DependencyObject element)
        {
            return (double)element.GetValue(HintOpacityProperty);
        }
        public static void SetHintOpacity(DependencyObject element, double value)
        {
            element.SetValue(HintOpacityProperty, value);
        }
        #endregion

    }
}
