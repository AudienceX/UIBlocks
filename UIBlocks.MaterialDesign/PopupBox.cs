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

        #region ToggleContent，按钮内容
        public static readonly DependencyProperty ToggleContentProperty = DependencyProperty.Register(
            "ToggleContent", typeof(object), typeof(PopupBox), new PropertyMetadata(default(object)));
        /// <summary>
        /// 按钮内容
        /// </summary>
        public object ToggleContent
        {
            get { return (object) GetValue(ToggleContentProperty); }
            set { SetValue(ToggleContentProperty, value); }
        }
        #endregion

        #region IsPopupOpen，是否弹出
        public static readonly DependencyProperty IsPopupOpenProperty = DependencyProperty.Register(
            "IsPopupOpen", typeof(bool), typeof(PopupBox), new PropertyMetadata(default(bool)));
        /// <summary>
        /// 是否弹出
        /// </summary>
        public bool IsPopupOpen
        {
            get { return (bool) GetValue(IsPopupOpenProperty); }
            set { SetValue(IsPopupOpenProperty, value); }
        }

        #endregion
    }
}
