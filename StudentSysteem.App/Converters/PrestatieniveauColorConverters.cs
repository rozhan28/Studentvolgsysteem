using System.Globalization;

namespace StudentSysteem.App.Converters
{
    // Hulpfunctie om een kleur resource op te zoeken
    public static class ResourceHelper
    {
        public static Color GetColorResource(string resourceKey)
        {
            if (Application.Current.Resources.TryGetValue(resourceKey, out object resourceValue))
            {
                if (resourceValue is Color color)
                {
                    return color;
                }
                // Optioneel: als de resource een SolidColorBrush is, haal dan de Color eruit
                if (resourceValue is SolidColorBrush brush)
                {
                    return brush.Color;
                }
            }
            return Colors.Transparent; 
        }
    }

    // Converter voor "In ontwikkeling" niveau
    public class InOntwikkelingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isInOntwikkeling && isInOntwikkeling)
            {
                System.Diagnostics.Debug.WriteLine("InOntwikkelingConverter - Retrieving 'InOntwikkeling' color from Resources");
                return ResourceHelper.GetColorResource("InOntwikkeling"); 
            }
            
            System.Diagnostics.Debug.WriteLine("InOntwikkelingConverter - Returning Transparent");
            return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Converter voor "Op niveau"
    public class OpNiveauConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isOpNiveau && isOpNiveau)
            {
                System.Diagnostics.Debug.WriteLine("OpNiveauConverter - Retrieving 'OpNiveau' color from Resources");
                return ResourceHelper.GetColorResource("OpNiveau");
            }
            
            System.Diagnostics.Debug.WriteLine("OpNiveauConverter - Returning Transparent");
            return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Converter voor "Boven niveau"
    public class BovenNiveauConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isBovenNiveau && isBovenNiveau)
            {
                System.Diagnostics.Debug.WriteLine("BovenNiveauConverter - Retrieving 'BovenNiveau' color from Resources");
                return ResourceHelper.GetColorResource("BovenNiveau");
            }
            
            System.Diagnostics.Debug.WriteLine("BovenNiveauConverter - Returning Transparent");
            return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}