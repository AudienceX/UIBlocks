using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace UIBlocks.MaterialDesignTheme.converter
{
    public class TextFieldHintVisibilityConverter : IValueConverter
    {
        public Visibility IsEmptyValue { get; set; } = Visibility.Visible;
        public Visibility IsNotEmptyValue { get; set; } = Visibility.Hidden;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty((value ?? "").ToString()) ? IsEmptyValue : IsNotEmptyValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
