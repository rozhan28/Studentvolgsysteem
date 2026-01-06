using System.Globalization;
using StudentSysteem.Core.Models;

namespace StudentSysteem.App.Converters;

public class FoutItemInLijstConverter : IMultiValueConverter
{
    private readonly ValidatieRandkleurConverter _kleurConverter = new();
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        // values[0] = De Toelichting van deze rij
        // values[1] = De HashSet met fouten (OngeldigeTekstVelden of OngeldigeOptieVelden)

        if (values?.Length >= 2 && values[0] is Toelichting item && values[1] is HashSet<Toelichting> foutenLijst)
        {
            // Geeft true terug als dit item in de foutenlijst staat
            bool isFout = foutenLijst.Contains(item);
            return _kleurConverter.Convert(isFout, typeof(Color), parameter, culture);
        }
        return Colors.LightGray;
    }
    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;
}