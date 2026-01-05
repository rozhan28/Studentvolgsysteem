using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace StudentSysteem.App.Converters
{
    public class BoolOmkeerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => !(bool)value;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => !(bool)value;
    }
}
