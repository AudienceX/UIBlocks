using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace UIBlocks.MaterialDesign
{
    public enum PopupBoxPlacementMode
    {
        BottomAndAlignLeftEdges,
        BottomAndAlignRightEdges,
        BottomAndAlignCentres,
        TopAndAlignLeftEdges,
        TopAndAlignRightEdges,
        TopAndAlignCentres,
        LeftAndAlignTopEdges,
        LeftAndAlignBottomEdges,
        LeftAndAlignMiddles,
        RightAndAlignTopEdges,
        RightAndAlignBottomEdges,
        RightAndAlignMiddles,
    }

    public enum PopupBoxPopupMode
    {
        Click,
        MouseOver,
        MouseOverEager
    }
    public class PopupBox : ContentControl
    {
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

        #region PopupContent,弹出内容
        public static readonly DependencyProperty PopupContentProperty = DependencyProperty.Register(
            "PopupContent", typeof(object), typeof(PopupBox), new PropertyMetadata(default(object)));
        /// <summary>
        ///  PopupContent,弹出内容
        /// </summary>
        public object PopupContent
        {
            get { return (object) GetValue(PopupContentProperty); }
            set { SetValue(PopupContentProperty, value); }
        }

        #endregion

        #region PopupContentTemplate

        public static readonly DependencyProperty PopupContentTemplateProperty = DependencyProperty.Register(
            "PopupContentTemplate", typeof(DataTemplate), typeof(PopupBox), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate PopupContentTemplate
        {
            get { return (DataTemplate) GetValue(PopupContentTemplateProperty); }
            set { SetValue(PopupContentTemplateProperty, value); }
        }

        #endregion

        private PopupEX _popup;
        private ContentControl _popupContentControl;
        private ToggleButton _togglubutton;

        static PopupBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PopupBox), new FrameworkPropertyMetadata(typeof(PopupBox)));
        }
    }
}
