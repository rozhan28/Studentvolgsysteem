using System.Globalization;

namespace StudentSysteem.App.Converters
{
    // Converter voor "In ontwikkeling" niveau
    public class InOntwikkelingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // DEBUG: Print de waarde naar console
            System.Diagnostics.Debug.WriteLine($"InOntwikkelingConverter - Value: {value}, Type: {value?.GetType()}");
            
            if (value is bool isInOntwikkeling && isInOntwikkeling)
            {
                System.Diagnostics.Debug.WriteLine("InOntwikkelingConverter - Returning BLUE");
                return Color.FromArgb("#4594D3");
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
            // DEBUG: Print de waarde naar console
            System.Diagnostics.Debug.WriteLine($"OpNiveauConverter - Value: {value}, Type: {value?.GetType()}");
            
            if (value is bool isOpNiveau && isOpNiveau)
            {
                System.Diagnostics.Debug.WriteLine("OpNiveauConverter - Returning YELLOW");
                return Color.FromArgb("#D5E05B");
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
            // DEBUG: Print de waarde naar console
            System.Diagnostics.Debug.WriteLine($"BovenNiveauConverter - Value: {value}, Type: {value?.GetType()}");
            
            if (value is bool isBovenNiveau && isBovenNiveau)
            {
                System.Diagnostics.Debug.WriteLine("BovenNiveauConverter - Returning GREEN");
                return Color.FromArgb("#45B97C");
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