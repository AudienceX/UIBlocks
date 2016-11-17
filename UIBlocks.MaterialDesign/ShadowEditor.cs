using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace UIBlocks.MaterialDesign
{
    public enum ShadowDepth
    {
        Depth0,
        Depth1,
        Depth2,
        Depth3,
        Depth4,
        Depth5
    }

    //用于存储原来的阴影信息，以便做动画变换
    internal class ShadowInfo
    {
        public ShadowInfo(double x)
        {
            ShadowOpacity = x;
        }

        public double ShadowOpacity { get; }
    }

    public static class ShadowEditor
    {
        /// <summary>
        ///     附加属性，设置阴影深度
        /// </summary>
        public static readonly DependencyProperty ShadowDepthProperty =
            DependencyProperty.RegisterAttached("ShadowDepth", typeof(ShadowDepth), typeof(ShadowEditor),
                new FrameworkPropertyMetadata(default(ShadowDepth), FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty DarkenProperty =
            DependencyProperty.RegisterAttached("Darken", typeof(bool), typeof(ShadowEditor),
                new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.AffectsRender,
                    DarkenPropertyChange));

        /// <summary>
        ///     附加属性，用于记录当前阴影透明度。
        /// </summary>
        private static readonly DependencyPropertyKey LocalInfoPropertyKey =
            DependencyProperty.RegisterAttachedReadOnly("LocalInfo", typeof(ShadowInfo), typeof(ShadowEditor),
                new PropertyMetadata(default(ShadowInfo)));

        public static ShadowDepth GetShadowDepth(DependencyObject obj)
        {
            return (ShadowDepth) obj.GetValue(ShadowDepthProperty);
        }

        public static void SetShadowDepth(DependencyObject obj, ShadowDepth value)
        {
            obj.SetValue(ShadowDepthProperty, value);
        }


        public static bool GetDarken(DependencyObject obj)
        {
            return (bool) obj.GetValue(DarkenProperty);
        }

        public static void SetDarken(DependencyObject obj, bool value)
        {
            obj.SetValue(DarkenProperty, value);
        }

        //Darken属性发生变化是的动画
        private static void DarkenPropertyChange(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var uielement = obj as UIElement;
            var dropshapdoweffect = uielement?.Effect as DropShadowEffect;
            if (dropshapdoweffect == null) return;
            if ((bool) e.NewValue)
            {
                SetLocalInfo(obj, new ShadowInfo(dropshapdoweffect.Opacity));
                var doubleAnimation = new DoubleAnimation(1, new Duration(TimeSpan.FromMilliseconds(350)))
                {
                    FillBehavior = FillBehavior.HoldEnd
                };
                dropshapdoweffect.BeginAnimation(DropShadowEffect.OpacityProperty, doubleAnimation);
            }
            else
            {
                var slocal = GetLocalInfo(obj);
                if (slocal == null) return;
                var doubleAnimation = new DoubleAnimation(slocal.ShadowOpacity,
                    new Duration(TimeSpan.FromMilliseconds(350)))
                {
                    FillBehavior = FillBehavior.HoldEnd
                };
                dropshapdoweffect.BeginAnimation(DropShadowEffect.OpacityProperty, doubleAnimation);
            }
        }


        private static ShadowInfo GetLocalInfo(DependencyObject obj)
        {
            return (ShadowInfo) obj.GetValue(LocalInfoPropertyKey.DependencyProperty);
        }

        private static void SetLocalInfo(DependencyObject obj, ShadowInfo value)
        {
            obj.SetValue(LocalInfoPropertyKey, value);
        }
    }
}