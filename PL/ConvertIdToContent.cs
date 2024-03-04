using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PL;

class ConvertIdToVisibilityForAdd : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertIdToVisibilityForUp : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertIdToIsVisibilityInEngineer : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == -1 ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertIdToIsEnableForId : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? true : false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
public class ConverterStringToDateTime : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime dateTime)
        {
            return dateTime.ToString("dd MMMM yyyy");
        }

        return "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class ConverterTimeSpanToDayes : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TimeSpan)
        {
            TimeSpan timeSpan = (TimeSpan)value;
            return timeSpan.TotalDays.ToString("0");
        }
        return "0"; // Default value if conversion fails
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string daysStr = value as string;
        if (int.TryParse(daysStr, out int days))
        {
            return new TimeSpan(days, 0, 0, 0);
        }
        return new TimeSpan(0, 0, 0, 0); // Default value if conversion fails
    }
}