using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace UIBlocks.MaterialDesign.Converters
{
    public class CardClipConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2 || !(values[0] is Size) || !(values[1] is Thickness))
                return Binding.DoNothing;

            var size = (Size)values[0];
            var farPoint = new Point(
                Math.Max(0, size.Width),
                Math.Max(0, size.Height));
            var padding = (Thickness)values[1];
            farPoint.Offset(padding.Left + padding.Right, padding.Top + padding.Bottom);

            return new Rect(
                new Point(),
                new Point(farPoint.X, farPoint.Y));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
