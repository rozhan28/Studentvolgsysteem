using System.Globalization;

namespace StudentSysteem.App.Converters
{
    public class ValidatieRandkleurConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isInvalid = (bool)value;

            // Bepaal welke Key gezocht moet worden in de Resources
            string colorKey = isInvalid ? "Validatie" : "Grey";

            if (Application.Current.Resources.TryGetValue(colorKey, out Object? color))
            {
                return (Color)color;
            }

            // Fallback voor het geval de keys niet gevonden worden in Colors.xaml
            return isInvalid ? Colors.Red : Colors.LightGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }
}