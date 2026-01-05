using System.Globalization;
using StudentSysteem.Core.Models;

namespace StudentSysteem.App.Converters;

public class CriteriumNaarTekst : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Criterium criterium)
        {
            return criterium.Beschrijving;
        }
            
        return "Toelichting gekoppeld aan...";
    }
    
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return string.Empty;
    }
}