using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace StudentSysteem.App.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isInvalid = (bool)value;

            return isInvalid ? Colors.Red : Colors.LightGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }
}