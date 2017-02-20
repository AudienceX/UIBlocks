using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace UIBlocks.MaterialDesignTheme
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

    internal class ShadowLocalInfo
    {
        public double StandardOpacity { get; }

        public ShadowLocalInfo(double standardopacity)
        {
            StandardOpacity = standardopacity;
        }
    }


    public static class ShadowEditor
    {
        #region shadowdepth,the depth of the shadow
        public static readonly DependencyProperty shadowdepthProperty = DependencyProperty.RegisterAttached(
    "shadowdepth", typeof(ShadowDepth), typeof(ShadowEditor), new FrameworkPropertyMetadata(default(ShadowDepth), FrameworkPropertyMetadataOptions.AffectsRender));
        public static void Setshadowdepth(DependencyObject element, ShadowDepth value)
        {
            element.SetValue(shadowdepthProperty, value);
        }
        public static ShadowDepth Getshadowdepth(DependencyObject element)
        {
            return (ShadowDepth)element.GetValue(shadowdepthProperty);
        }
        #endregion

        #region LocalInfo,the opacity of the current ripple
        private static readonly DependencyProperty LocalInfoProperty = DependencyProperty.RegisterAttached(
    "LocalInfo", typeof(ShadowLocalInfo), typeof(ShadowEditor), new PropertyMetadata(default(ShadowLocalInfo)));
        private static void SetLocalInfo(DependencyObject element, ShadowLocalInfo value)
        {
            element.SetValue(LocalInfoProperty, value);
        }
        private static ShadowLocalInfo GetLocalInfo(DependencyObject element)
        {
            return (ShadowLocalInfo)element.GetValue(LocalInfoProperty);
        }
        #endregion

        #region Darken,make the shadow deep
        public static readonly DependencyProperty DarkenProperty = DependencyProperty.RegisterAttached(
    "Darken", typeof(bool), typeof(ShadowEditor), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.AffectsRender, DarkenPropertyChangedCallback));
        private static void DarkenPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var uiElement = dependencyObject as UIElement;
            var dropShadowEffect = uiElement?.Effect as DropShadowEffect;

            if (dropShadowEffect == null) return;

            if ((bool)dependencyPropertyChangedEventArgs.NewValue)
            {
                SetLocalInfo(dependencyObject, new ShadowLocalInfo(dropShadowEffect.Opacity));

                var doubleAnimation = new DoubleAnimation(1, new Duration(TimeSpan.FromMilliseconds(350)))
                {
                    FillBehavior = FillBehavior.HoldEnd
                };
                dropShadowEffect.BeginAnimation(DropShadowEffect.OpacityProperty, doubleAnimation);
            }
            else
            {
                var shadowLocalInfo = GetLocalInfo(dependencyObject);
                if (shadowLocalInfo == null) return;

                var doubleAnimation = new DoubleAnimation(shadowLocalInfo.StandardOpacity, new Duration(TimeSpan.FromMilliseconds(350)))
                {
                    FillBehavior = FillBehavior.HoldEnd
                };
                dropShadowEffect.BeginAnimation(DropShadowEffect.OpacityProperty, doubleAnimation);
            }
        }
        public static void SetDarken(DependencyObject element, bool value)
        {
            element.SetValue(DarkenProperty, value);
        }
        public static bool GetDarken(DependencyObject element)
        {
            return (bool)element.GetValue(DarkenProperty);
        }
        #endregion

    }
}
