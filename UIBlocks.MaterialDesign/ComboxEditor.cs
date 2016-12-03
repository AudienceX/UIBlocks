using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UIBlocks.MaterialDesign
{
    public static class ComboxEditor
    {
        #region ShowSelectedItem

        /// <summary>
        /// By default the selected item his hidden from the drop down list, as per Material Design specifications. 
        /// To revert to a more classic Windows desktop behaviour, and show the currently selected item again in the drop
        /// down, set this attached propety to true.
        /// </summary>
        public static readonly DependencyProperty ShowSelectedItemProperty = DependencyProperty.RegisterAttached(
            "ShowSelectedItem",
            typeof(bool),
            typeof(ComboxEditor),
            new FrameworkPropertyMetadata(false,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

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
