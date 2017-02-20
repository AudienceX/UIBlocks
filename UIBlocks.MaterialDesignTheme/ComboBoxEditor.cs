using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UIBlocks.MaterialDesignTheme
{
    public static  class ComboBoxEditor
    {
        #region ShowSelectedItem,附加属性，是否在Combox弹出内容中显示已选中项，默认不显示。
        public static readonly DependencyProperty ShowSelectedItemProperty = DependencyProperty.RegisterAttached(
            "ShowSelectedItem",typeof(bool),typeof(ComboBoxEditor),new FrameworkPropertyMetadata(false,FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
        public static bool GetShowSelectedItem(DependencyObject element, object value)
        {
            return (bool)element.GetValue(ShowSelectedItemProperty);
        }
        public static void SetShowSelectedItem(DependencyObject element, object value)
        {
            element.SetValue(ShowSelectedItemProperty, value);
        }
        #endregion

    }
}
