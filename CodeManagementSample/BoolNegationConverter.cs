using System;
using System.Globalization;
using System.Windows.Data;

namespace CodeManagementSample;

/// <summary>
/// Negates a boolean binding value.
/// </summary>
public class BoolNegationConverter : IValueConverter
{
    /// <inheritdoc />
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool b)
        {
            return !b;
        }

        return true;
    }

    /// <inheritdoc />
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}