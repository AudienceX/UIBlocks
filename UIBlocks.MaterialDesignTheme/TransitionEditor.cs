using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UIBlocks.MaterialDesignTheme
{
    public static class TransitionEditor
    {
        /// <summary>
        /// 依赖属性DisableTransitions，转换是否被禁止
        /// </summary>
        public static readonly DependencyProperty DisableTransitionsProperty = DependencyProperty.RegisterAttached(
            "DisableTransitions", typeof(bool), typeof(TransitionEditor), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.Inherits));
        public static void SetDisableTransitions(DependencyObject element, bool value)
        {
            element.SetValue(DisableTransitionsProperty, value);
        }
        public static bool GetDisableTransitions(DependencyObject element)
        {
            return (bool)element.GetValue(DisableTransitionsProperty);
        }
    }
}
