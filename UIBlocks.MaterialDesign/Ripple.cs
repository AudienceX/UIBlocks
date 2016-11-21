using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace UIBlocks.MaterialDesign
{
    //产生波纹的类，继承自ContentControl，使用时当一般的ContenControl加入控件模板中即可，如何控制具体状态（如是否从中心发出、关闭等）可由静态类RippleEditor设置附加属性定义。
    public class Ripple : ContentControl
    {
        private static readonly HashSet<Ripple> instance = new HashSet<Ripple>();

        //静态初始化类，注册鼠标释放事件。
        static Ripple()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Ripple), new FrameworkPropertyMetadata(typeof(Ripple)));
            EventManager.RegisterClassHandler(typeof(ContentControl), Mouse.PreviewMouseUpEvent,
                new MouseButtonEventHandler(MouseUpHandler), true);
        }

        public Ripple()
        {
            SizeChanged += OnSizeChanged;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //设置初始状态
            VisualStateManager.GoToState(this, "normal", false);
        }


        //鼠标释放，加速结束波纹动画
        private static void MouseUpHandler(object sender, MouseEventArgs e)
        {
            foreach (var i in instance)
            {
                var scalet = i.Template.FindName("scaletransform", i) as ScaleTransform;
                if (scalet != null)
                {
                    var currentscale = scalet.ScaleX;
                    var newtime = TimeSpan.FromMilliseconds(300*(1.0 - currentscale));

                    var scaleXKeyFrame = i.Template.FindName("xkeyframe", i) as EasingDoubleKeyFrame;
                    if (scaleXKeyFrame != null)
                        scaleXKeyFrame.KeyTime = KeyTime.FromTimeSpan(newtime);

                    var scaleYKeyFrame = i.Template.FindName("ykeyframe", i) as EasingDoubleKeyFrame;
                    if (scaleYKeyFrame != null)
                        scaleYKeyFrame.KeyTime = KeyTime.FromTimeSpan(newtime);
                }
                VisualStateManager.GoToState(i, "normal", true);
            }
            instance.Clear();
        }

        //计算波纹大小
        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            var innerContent = Content as FrameworkElement;

            double width, height;

            if (RippleEditor.GetIsInCenter(this) && (innerContent != null))
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
            size = 2*radius*1.0;
        }

        //鼠标单击，开始波纹动画。
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            var point = e.GetPosition(this);
            if (RippleEditor.GetIsInCenter(this))
            {
                var content = Content as FrameworkElement;
                if (content != null)
                {
                    var position = TransformToAncestor(this).Transform(new Point(0, 0));
                    X = position.X + content.ActualWidth/2 - size/2;
                    Y = position.Y + content.ActualHeight/2 - size/2;
                }
                else
                {
                    X = ActualWidth/2 - size/2;
                    Y = ActualHeight/2 - size/2;
                }
            }
            else
            {
                X = point.X - size/2;
                Y = point.Y - size/2;
            }
            if (RippleEditor.GetEnable(this))
            {
                VisualStateManager.GoToState(this, "normal", false);
                VisualStateManager.GoToState(this, "click", true);
                instance.Add(this);
            }
            base.OnMouseLeftButtonDown(e);
        }

        #region  //各种属性

        public double X
        {
            get { return (double) GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        /// <summary>
        ///     依赖属性，波纹中心X坐标
        /// </summary>
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(double), typeof(Ripple), new PropertyMetadata(default(double)));

        public double Y
        {
            get { return (double) GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        /// <summary>
        ///     依赖属性，波纹中心Y坐标
        /// </summary>
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(double), typeof(Ripple), new PropertyMetadata(default(double)));

        public double size
        {
            get { return (double) GetValue(sizeProperty); }
            set { SetValue(sizeProperty, value); }
        }

        /// <summary>
        ///     依赖属性，波纹大小
        /// </summary>
        public static readonly DependencyProperty sizeProperty =
            DependencyProperty.Register("size", typeof(double), typeof(Ripple), new PropertyMetadata(default(double)));

        #endregion
    }
}