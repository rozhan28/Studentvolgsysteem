using System.Globalization;
using StudentSysteem.Core.Models;

namespace StudentSysteem.App.Converters
{
    public class PrestatieniveauKleurConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Niveauaanduiding geselecteerdNiveau && parameter is string verwachtNiveau)
            {
                System.Diagnostics.Debug.WriteLine($"Converter: geselecteerd={geselecteerdNiveau}, verwacht={verwachtNiveau}");
                
                if (Enum.TryParse<Niveauaanduiding>(verwachtNiveau, out var verwacht))
                {
                    if (geselecteerdNiveau == verwacht)
                    {
                        return verwacht switch
                        {
                            Niveauaanduiding.InOntwikkeling => GetColorResource("InOntwikkeling"),
                            Niveauaanduiding.OpNiveau => GetColorResource("OpNiveau"),
                            Niveauaanduiding.BovenNiveau => GetColorResource("BovenNiveau"),
                            _ => GetColorResource("NietIngeleverd")
                        };
                    }
                }
            }
            return GetColorResource("NietIngeleverd");
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