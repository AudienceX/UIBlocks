using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace UIBlocks.MaterialDesignTheme
{
    /// <summary>
    /// 实现波纹效果。
    /// </summary>
    public class Ripple:ContentControl
    {
        private static readonly HashSet<Ripple> instance=new HashSet<Ripple>();

        static Ripple()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Ripple),new FrameworkPropertyMetadata(typeof(Ripple)));
            EventManager.RegisterClassHandler(typeof(ContentControl),Mouse.PreviewMouseUpEvent,new MouseButtonEventHandler(MouseUpHandler),true);
            EventManager.RegisterClassHandler(typeof(ContentControl), Mouse.MouseMoveEvent, new MouseEventHandler(MouseMouveHandler), true);
        }

        private static void MouseMouveHandler(object sender, MouseEventArgs e)
        {
            foreach (var ripple in instance.ToList())
            {
                var relativePosition = Mouse.GetPosition(ripple);
                if (relativePosition.X < 0
                    || relativePosition.Y < 0
                    || relativePosition.X >= ripple.ActualWidth
                    || relativePosition.Y >= ripple.ActualHeight)
                {
                    VisualStateManager.GoToState(ripple, "mouseout", true);
                    instance.Remove(ripple);
                }
            }
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            var point = e.GetPosition(this);
            if (RippleEditor.GetOnCenter(this))
            {
                var innercontent = Content as FrameworkElement;
                if (innercontent != null)
                {
                    var position = innercontent.TransformToAncestor(this).Transform(new Point(0, 0));
                    offsetx = position.X + innercontent.ActualWidth / 2 - RippleSize / 2;
                    offsety = position.Y + innercontent.ActualHeight / 2 - RippleSize / 2;
                }
                else
                {
                    offsetx = ActualWidth / 2 - RippleSize / 2;
                    offsety = ActualHeight / 2 - RippleSize / 2;
                }
            }
            else
            {
                offsetx = point.X - RippleSize / 2;
                offsety = point.Y - RippleSize / 2;
            }
            if (!RippleEditor.Getdisable(this))
            {
                VisualStateManager.GoToState(this, "normal", false);
                VisualStateManager.GoToState(this, "click", true);
                instance.Add(this);
            }
            base.OnPreviewMouseLeftButtonDown(e);
        }

        private static void MouseUpHandler(object sender, MouseEventArgs e)
        {
            foreach (var ripple in instance)
            {
                var scaleTrans = ripple.Template.FindName("scale", ripple) as ScaleTransform;
                if (scaleTrans != null)
                {
                    double currentScale = scaleTrans.ScaleX;
                    var newTime = TimeSpan.FromMilliseconds(300 * (1.0 - currentScale));

                    var scaleXKeyFrame = ripple.Template.FindName("clicktonormalxkey", ripple) as EasingDoubleKeyFrame;
                    if (scaleXKeyFrame != null)
                    {
                        scaleXKeyFrame.KeyTime = KeyTime.FromTimeSpan(newTime);
                    }
                    var scaleYKeyFrame = ripple.Template.FindName("clicktonormalykey", ripple) as EasingDoubleKeyFrame;
                    if (scaleYKeyFrame != null)
                    {
                        scaleYKeyFrame.KeyTime = KeyTime.FromTimeSpan(newTime);
                    }
                }
                ScaleTransform a= (ScaleTransform)ripple.Template.FindName("scale",ripple);
                Console.WriteLine(a?.ScaleX);
                VisualStateManager.GoToState(ripple, "normal", true);
                Console.WriteLine(RippleSizeProperty.ToString());
            }
            instance.Clear();
        }

        public Ripple()
        {
            SizeChanged += Ripple_SizeChanged;   
        }

        private void Ripple_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var innerContent = (Content as FrameworkElement);
            double width, height;
            if (RippleEditor.GetOnCenter(this) && innerContent != null)
            {
                width = innerContent.ActualWidth;
                height = innerContent.ActualHeight;
            }
            else
            {
                width = e.NewSize.Width;
                height = e.NewSize.Height;
            }
            var radius = Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2));
            RippleSize = 2*radius;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            VisualStateManager.GoToState(this, "normal", false);
        }

        #region RippleSize,the size of the ripple
        public static readonly DependencyProperty RippleSizeProperty = DependencyProperty.Register(
    "RippleSize", typeof(double), typeof(Ripple), new PropertyMetadata(default(double)));

        public double RippleSize
        {
            get { return (double)GetValue(RippleSizeProperty); }
            set { SetValue(RippleSizeProperty, value); }
        }
        #endregion

        #region FeedBackBrush,which defines the color of the ripple.
        public static readonly DependencyProperty FeedBackBrushProperty = DependencyProperty.Register(
    nameof(FeedBackBrush), typeof(Brush), typeof(Ripple), new PropertyMetadata(default(Brush)));
        public Brush FeedBackBrush
        {
            get { return (Brush)GetValue(FeedBackBrushProperty); }
            set { SetValue(FeedBackBrushProperty, value); }
        }
        #endregion

        #region offsetx,水平偏移
        public static readonly DependencyProperty offsetxProperty = DependencyProperty.Register(
    "offsetx", typeof(double), typeof(Ripple), new PropertyMetadata(default(double)));
        public double offsetx
        {
            get { return (double)GetValue(offsetxProperty); }
            set { SetValue(offsetxProperty, value); }
        }
        #endregion

        #region offsety,垂直偏移
        public static readonly DependencyProperty offsetyProperty = DependencyProperty.Register(
    "offsety", typeof(double), typeof(Ripple), new PropertyMetadata(default(double)));
        public double offsety
        {
            get { return (double)GetValue(offsetyProperty); }
            set { SetValue(offsetyProperty, value); }
        }
        #endregion
    }
}
