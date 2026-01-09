using System.Globalization;
using StudentSysteem.Core.Models;

namespace StudentSysteem.App.Converters
{
    public class PrestatieniveauKleurConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not Niveauaanduiding geselecteerd || parameter == null)
                return GetColorResource("Secondary");

            if (!Enum.TryParse(parameter.ToString(), out Niveauaanduiding blokNiveau))
                return GetColorResource("Secondary");

            return geselecteerd == blokNiveau
                ? GetColorResource(blokNiveau.ToString())
                : GetColorResource("Secondary");
        }


        private static Color GetColorResource(string resourceKey)
        {
            if (Application.Current?.Resources != null && 
                Application.Current.Resources.TryGetValue(resourceKey, out object resourceValue))
            {
                if (resourceValue is Color color)
                {
                    return color;
                }
                if (resourceValue is SolidColorBrush brush)
                {
                    return brush.Color;
                }
            }
            return Colors.Transparent; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}