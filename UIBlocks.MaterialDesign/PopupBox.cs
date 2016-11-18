using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace UIBlocks.MaterialDesign
{
    public class PopupBox:ContentControl
    {
        static  PopupBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PopupBox), new FrameworkPropertyMetadata(typeof(PopupBox)));
        }

        #region ButtonConten

        public static readonly DependencyProperty ButtonContentProperty = DependencyProperty.Register(
            "ButtonContent", typeof(object), typeof(PopupBox), new PropertyMetadata(default(object)));

        public object ButtonContent
        {
            get { return (object) GetValue(ButtonContentProperty); }
            set { SetValue(ButtonContentProperty, value); }
        }
        #endregion
    }
}
