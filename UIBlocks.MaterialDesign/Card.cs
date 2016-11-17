using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UIBlocks.MaterialDesign
{
    public class Card : ContentControl
    {
        private Border clipborder;

        static Card()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Card), new FrameworkPropertyMetadata(typeof(Card)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            clipborder = Template.FindName("ClipBorder", this) as Border;
            Console.WriteLine(clipborder.ToString());
        }


        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            if (clipborder == null) return;

            var farPoint = new Point(
                Math.Max(0, clipborder.ActualWidth),
                Math.Max(0, clipborder.ActualHeight));

            var clipRect = new Rect(
                new Point(),
                new Point(farPoint.X, farPoint.Y));

            ClipContent = new RectangleGeometry(clipRect, Cornerradius, Cornerradius);
        }

        #region 依赖属性CornerRadius，卡片圆角

        public double Cornerradius
        {
            get { return (double) GetValue(CornerradiusProperty); }
            set { SetValue(CornerradiusProperty, value); }
        }

        public static readonly DependencyProperty CornerradiusProperty =
            DependencyProperty.RegisterAttached("Cornerradius", typeof(double), typeof(Card),
                new FrameworkPropertyMetadata(2.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        #endregion

        #region ClipContent依赖属性，卡牌内容形状

        public Geometry ClipContent
        {
            get { return (Geometry) GetValue(ClipContentProperty); }
            set { SetValue(ClipContentProperty, value); }
        }

        public static readonly DependencyProperty ClipContentProperty =
            DependencyProperty.RegisterAttached("ClipContent", typeof(Geometry), typeof(Card),
                new PropertyMetadata(default(Geometry)));

        #endregion
    }
}