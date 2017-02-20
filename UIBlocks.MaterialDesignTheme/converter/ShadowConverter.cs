using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Effects;

namespace UIBlocks.MaterialDesignTheme.converter
{
    public class ShadowConverter : IValueConverter
    {

        private static readonly IDictionary<ShadowDepth, DropShadowEffect> ShadowDictionary;
        public static readonly ShadowConverter instance = new ShadowConverter();

        static ShadowConverter()
        {
            var resdic = new ResourceDictionary { Source = new Uri("pack://application:,,,/UIBlocks.MaterialDesignTheme;component/Themes/Shadows.xaml", UriKind.Absolute) };
            ShadowDictionary = new Dictionary<ShadowDepth, DropShadowEffect>
            {
                {ShadowDepth.Depth0, null },
                {ShadowDepth.Depth1, (DropShadowEffect)resdic["MaterialDesignShadowDepth1"] },
                {ShadowDepth.Depth2, (DropShadowEffect)resdic["MaterialDesignShadowDepth2"] },
                {ShadowDepth.Depth3, (DropShadowEffect)resdic["MaterialDesignShadowDepth3"] },
                {ShadowDepth.Depth4, (DropShadowEffect)resdic["MaterialDesignShadowDepth4"] },
                {ShadowDepth.Depth5, (DropShadowEffect)resdic["MaterialDesignShadowDepth5"] },
            };
        }

        private static DropShadowEffect Clone(DropShadowEffect dropShadowEffect)
        {
            if (dropShadowEffect == null) return null;
            return new DropShadowEffect()
            {
                BlurRadius = dropShadowEffect.BlurRadius,
                Color = dropShadowEffect.Color,
                Direction = dropShadowEffect.Direction,
                Opacity = dropShadowEffect.Opacity,
                RenderingBias = dropShadowEffect.RenderingBias,
                ShadowDepth = dropShadowEffect.ShadowDepth
            };
        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ShadowDepth)) return null;
            return (DropShadowEffect)Clone(ShadowDictionary[(ShadowDepth)value]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
