using System.Globalization;

namespace StudentSysteem.App.Converters
{
    public class ValidatieRandkleurConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isInvalid = (bool)value;
            string standardColor = parameter as string ?? "Grey";
            
            // Bepaal welke Key gezocht moet worden in de Resources
            string colorKey = isInvalid ? "Validatie" : standardColor;

            if (Application.Current.Resources.TryGetValue(colorKey, out var color))
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