using Microsoft.UI.Xaml.Data;

namespace App.Converters;

public class TimeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return new DateTimeOffset(((DateTime)value).ToUniversalTime());

    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value == null) return DateTime.MinValue;
        return ((DateTimeOffset)value).DateTime;
    }
}