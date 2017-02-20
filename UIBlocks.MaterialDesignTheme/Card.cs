using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UIBlocks.MaterialDesignTheme
{
    public class Card:ContentControl
    {
        public const string clipbordername = "clipborder";
        private Border _clipborder;
        static Card()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Card),new FrameworkPropertyMetadata(typeof(Card)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _clipborder=Template.FindName(clipbordername,this) as Border;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo info)
        {
            base.OnRenderSizeChanged(info);
            if(_clipborder==null) return;
            var farpoint=new Point(Math.Max(0,_clipborder.ActualWidth),Math.Max(0,_clipborder.ActualHeight));
            var cliprect=new Rect(new Point(),new Point(farpoint.X,farpoint.Y));
            ContentClip=new RectangleGeometry(cliprect,CardCornerRadius,CardCornerRadius);
        }

        #region CardCornerRadius,卡片的圆角弧度
        public static readonly DependencyProperty CardCornerRadiusProperty = DependencyProperty.Register(
    "CardCornerRadius", typeof(double), typeof(Card), new FrameworkPropertyMetadata(2.0, FrameworkPropertyMetadataOptions.AffectsMeasure));
        public double CardCornerRadius
        {
            get { return (double)GetValue(CardCornerRadiusProperty); }
            set { SetValue(CardCornerRadiusProperty, value); }
        }
        #endregion

        #region ContentClip，卡片Clip属性
        public static readonly DependencyProperty ContentClipProperty = DependencyProperty.Register(
    "ContentClip", typeof(Geometry), typeof(Card), new PropertyMetadata(default(Geometry)));
        public Geometry ContentClip
        {
            get { return (Geometry)GetValue(ContentClipProperty); }
            set { SetValue(ContentClipProperty, value); }
        }
        #endregion

    }
}
