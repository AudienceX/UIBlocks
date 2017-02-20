using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace UIBlocks.MaterialDesignTheme
{
    public static class TextFieldEditor
    {
        #region DecorationVisibility,附加属性，控制下划线是否可见。
        public static readonly DependencyProperty DecorationVisibilityProperty = DependencyProperty.RegisterAttached(
            "DecorationVisibility", typeof(Visibility), typeof(TextFieldEditor), new PropertyMetadata(default(Visibility)));
        public static void SetDecorationVisibility(DependencyObject element, Visibility value)
        {
            element.SetValue(DecorationVisibilityProperty, value);
        }
        public static Visibility GetDecorationVisibility(DependencyObject element)
        {
            return (Visibility)element.GetValue(DecorationVisibilityProperty);
        }
        #endregion

        #region TextBoxViewMargin，附加属性，文本框Marign
        public static readonly DependencyProperty TextBoxViewMarginProperty = DependencyProperty.RegisterAttached(
"TextBoxViewMargin",
typeof(Thickness), typeof(TextFieldEditor),
new FrameworkPropertyMetadata(new Thickness(double.NegativeInfinity), FrameworkPropertyMetadataOptions.Inherits, TextBoxViewMarginPropertyChangedCallback));
        public static void SetTextBoxViewMargin(DependencyObject element, Thickness value)
        {
            element.SetValue(TextBoxViewMarginProperty, value);
        }
        public static Thickness GetTextBoxViewMargin(DependencyObject element)
        {
            return (Thickness)element.GetValue(TextBoxViewMarginProperty);
        }
        private static void TextBoxViewMarginPropertyChangedCallback(DependencyObject dependencyObject,DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var box = dependencyObject as Control; //could be a text box or password box
            if (box == null)
            {
                return;
            }

            if (box.IsLoaded)
            {
                ApplyTextBoxViewMargin(box, (Thickness)dependencyPropertyChangedEventArgs.NewValue);
            }

            box.Loaded += (sender, args) =>
            {
                var textBox = (Control)sender;
                ApplyTextBoxViewMargin(textBox, GetTextBoxViewMargin(textBox));
            };
        }

        #endregion
        private static void ApplyTextBoxViewMargin(Control textBox, Thickness margin)
        {
            if (margin.Equals(new Thickness(double.NegativeInfinity)))
            {
                return;
            }
            if (textBox.Template == null) return;
            var frameworkElement = (textBox.Template.FindName("PART_ContentHost", textBox) as ScrollViewer)?.Content as FrameworkElement;
            if (frameworkElement != null)
            {
                frameworkElement.Margin = margin;
            }
        }


    }
}
