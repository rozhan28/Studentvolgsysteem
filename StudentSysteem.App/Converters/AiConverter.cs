using System.Globalization;

namespace StudentSysteem.App.Converters;

public class AiConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var scale = value as string;

        return scale switch
        {
            "Exploratie" => "ai_samenwerken.png",
            "Geen" => "ai_geen.png",
            "Planning" => "ai_planning.png",
            "Samenwerking" => "ai_samenwerking.png",
            "Volledig" => "ai_volledig.png",
            _ => null
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}