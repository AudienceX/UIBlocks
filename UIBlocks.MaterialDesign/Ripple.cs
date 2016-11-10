using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UIBlocks.MaterialDesign
{
    public class Ripple : ContentControl
    {
        private static readonly HashSet<Ripple> instance = new HashSet<Ripple>();

        #region  //各种属性
        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }
        /// <summary>
        ///依赖属性，波纹中心X坐标
        ///</summary>
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(double), typeof(Ripple), new PropertyMetadata(default(double)));

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }
        /// <summary>
        ///依赖属性，波纹中心Y坐标
        ///</summary>
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(double), typeof(Ripple), new PropertyMetadata(default(double)));

        public double size
        {
            get { return (double)GetValue(sizeProperty); }
            set { SetValue(sizeProperty, value); }
        }
        /// <summary>
        ///   依赖属性，波纹大小
        /// </summary>
        public static readonly DependencyProperty sizeProperty =
            DependencyProperty.Register("size", typeof(double), typeof(Ripple), new PropertyMetadata(default(double)));
        #endregion

        static Ripple()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Ripple), new FrameworkPropertyMetadata(typeof(Ripple)));
            EventManager.RegisterClassHandler(typeof(ContentControl), Mouse.PreviewMouseUpEvent, new MouseButtonEventHandler(MouseUpHandler), true);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            VisualStateManager.GoToState(this, "normal", false);
        }

        private static void MouseUpHandler(object sender, MouseEventArgs e)
        {
            foreach (var i in instance)
            {
                var scalet = i.Template.FindName("scaletransform", i) as ScaleTransform;
                if (scalet != null)
                {
                    double currentscale = scalet.ScaleX;
                    var newtime = TimeSpan.FromMilliseconds(300 * (1.0 - currentscale));

                    var scaleXKeyFrame = i.Template.FindName("xkeyframe", i) as EasingDoubleKeyFrame;
                    if (scaleXKeyFrame != null)
                    {
                        scaleXKeyFrame.KeyTime = KeyTime.FromTimeSpan(newtime);
                    }

                    var scaleYKeyFrame = i.Template.FindName("ykeyframe", i) as EasingDoubleKeyFrame;
                    if (scaleYKeyFrame != null)
                    {
                        scaleYKeyFrame.KeyTime = KeyTime.FromTimeSpan(newtime);
                    }
                }
                VisualStateManager.GoToState(i, "normal", true);
            }
            instance.Clear();
        }

        public Ripple()
        {
            SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            var innerContent = (Content as FrameworkElement);

            double width, height;

            if (RippleEditor.GetIsInCenter(this) && innerContent != null)
            {
                width = innerContent.ActualWidth;
                height = innerContent.ActualHeight;
            }
            else
            {
                width = sizeChangedEventArgs.NewSize.Width;
                height = sizeChangedEventArgs.NewSize.Height;
            }

            var radius = Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2));

            size = 2 * radius * 1.0;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            var point = e.GetPosition(this);
            if (RippleEditor.GetIsInCenter(this))
            {
                var content = Content as FrameworkElement;
                if (content != null)
                {
                    var position = TransformToAncestor(this).Transform(new Point(0, 0));
                    X = position.X + content.ActualWidth / 2 - size / 2;
                    Y = position.Y + content.ActualHeight / 2 - size / 2;
                }
                else
                {
                    X = ActualWidth / 2 - size / 2;
                    Y = ActualHeight / 2 - size / 2;
                }
            }
            else
            {
                X = point.X - size / 2;
                Y = point.Y - size / 2;
            }
            if (RippleEditor.GetEnable(this))
            {
                VisualStateManager.GoToState(this, "normal", false);
                VisualStateManager.GoToState(this, "click", true);
                instance.Add(this);
            }
            base.OnMouseLeftButtonDown(e);
        }
    }
}
