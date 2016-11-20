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

        #region TogggleContentTemplate

        public static readonly DependencyProperty ToggleContentTemplateProperty = DependencyProperty.Register(
            "ToggleContentTemplate", typeof(DataTemplate), typeof(PopupBox), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate ToggleContentTemplate
        {
            get { return (DataTemplate) GetValue(ToggleContentTemplateProperty); }
            set { SetValue(ToggleContentTemplateProperty, value); }
        }

        #endregion

        #region ToggleCheckedContent

        public static readonly DependencyProperty ToggleCheckedContentProperty = DependencyProperty.Register(
            "ToggleCheckedContent", typeof(object), typeof(PopupBox), new PropertyMetadata(default(object)));

        public object ToggleCheckedContent
        {
            get { return (object) GetValue(ToggleCheckedContentProperty); }
            set { SetValue(ToggleCheckedContentProperty, value); }
        }

        #endregion

        #region ToggleCheckedContentTemplate
        public static readonly DependencyProperty ToggleCheckedContentTempalteProperty = DependencyProperty.Register(
            "ToggleCheckedContentTempalte", typeof(DataTemplate), typeof(PopupBox), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate ToggleCheckedContentTempalte
        {
            get { return (DataTemplate) GetValue(ToggleCheckedContentTempalteProperty); }
            set { SetValue(ToggleCheckedContentTempalteProperty, value); }
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

        #region PlacementMode

        public static readonly DependencyProperty PlacementModeProperty = DependencyProperty.Register(
            "PlacementMode", typeof(PopupBoxPlacementMode), typeof(PopupBox), new PropertyMetadata(default(PopupBoxPlacementMode)));

        public PopupBoxPlacementMode PlacementMode
        {
            get { return (PopupBoxPlacementMode) GetValue(PlacementModeProperty); }
            set { SetValue(PlacementModeProperty, value); }
        }

        #endregion

        #region StayOpen

        public static readonly DependencyProperty StayOpenProperty = DependencyProperty.Register(
            "StayOpen", typeof(bool), typeof(PopupBox), new PropertyMetadata(default(bool)));
        /// <summary>
        /// 
        /// </summary>
        public bool StayOpen
        {
            get { return (bool) GetValue(StayOpenProperty); }
            set { SetValue(StayOpenProperty, value); }
        }

        #endregion

        private PopupEX _popup;
        private ContentControl _popupContentControl;
        private ToggleButton _togglubutton;
        private Point _popupPoint;

        static PopupBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PopupBox), new FrameworkPropertyMetadata(typeof(PopupBox)));
        }

        public CustomPopupPlacementCallback PopupPlacementMethod => GetPopupPlacement;


        private CustomPopupPlacement[] GetPopupPlacement(Size popupSize, Size targetSize, Point offset)
        {
            double x, y;
            if (FlowDirection == FlowDirection.LeftToRight)
            {
                offset.X += targetSize.Width/2;
            }

            switch (PlacementMode)
            {
                case PopupBoxPlacementMode.BottomAndAlignLeftEdges:
                    x = 0 - Math.Abs(offset.X * 3);
                    y = targetSize.Height - Math.Abs(offset.Y);
                    break;
                case PopupBoxPlacementMode.BottomAndAlignRightEdges:
                    x = 0 - popupSize.Width + targetSize.Width - offset.X;
                    y = targetSize.Height - Math.Abs(offset.Y);
                    break;
                case PopupBoxPlacementMode.BottomAndAlignCentres:
                    x = targetSize.Width / 2 - popupSize.Width / 2 - Math.Abs(offset.X * 2);
                    y = targetSize.Height - Math.Abs(offset.Y);
                    break;
                case PopupBoxPlacementMode.TopAndAlignLeftEdges:
                    x = 0 - Math.Abs(offset.X * 3);
                    y = 0 - popupSize.Height - Math.Abs(offset.Y * 2);
                    break;
                case PopupBoxPlacementMode.TopAndAlignRightEdges:
                    x = 0 - popupSize.Width + targetSize.Width - offset.X;
                    y = 0 - popupSize.Height - Math.Abs(offset.Y * 2);
                    break;
                case PopupBoxPlacementMode.TopAndAlignCentres:
                    x = targetSize.Width / 2 - popupSize.Width / 2 - Math.Abs(offset.X * 2);
                    y = 0 - popupSize.Height - Math.Abs(offset.Y * 2);
                    break;
                case PopupBoxPlacementMode.LeftAndAlignTopEdges:
                    x = 0 - popupSize.Width - Math.Abs(offset.X * 2);
                    y = 0 - Math.Abs(offset.Y * 3);
                    break;
                case PopupBoxPlacementMode.LeftAndAlignBottomEdges:
                    x = 0 - popupSize.Width - Math.Abs(offset.X * 2);
                    y = 0 - (popupSize.Height - targetSize.Height);
                    break;
                case PopupBoxPlacementMode.LeftAndAlignMiddles:
                    x = 0 - popupSize.Width - Math.Abs(offset.X * 2);
                    y = targetSize.Height / 2 - popupSize.Height / 2 - Math.Abs(offset.Y * 2);
                    break;
                case PopupBoxPlacementMode.RightAndAlignTopEdges:
                    x = targetSize.Width;
                    y = 0 - Math.Abs(offset.X * 3);
                    break;
                case PopupBoxPlacementMode.RightAndAlignBottomEdges:
                    x = targetSize.Width;
                    y = 0 - (popupSize.Height - targetSize.Height);
                    break;
                case PopupBoxPlacementMode.RightAndAlignMiddles:
                    x = targetSize.Width;
                    y = targetSize.Height / 2 - popupSize.Height / 2 - Math.Abs(offset.Y * 2);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _popupPoint=new Point(x,y);
            return new[] { new CustomPopupPlacement(_popupPoint, PopupPrimaryAxis.Horizontal) };
        }
    }
}
