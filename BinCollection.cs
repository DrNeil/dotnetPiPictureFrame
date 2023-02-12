using System;
using System.Globalization;
using Avalonia.Data.Converters;


/// <summary>
/// This is a service class that is used to retrieve the bin collection dates and types for a given address
/// This is only working for the NSW Northern Beaches Council area
/// </summary>
internal class BinCollectionConverter: IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
            return null;

        if (value is Bins.BinCollection binCollection)
        {
            return $"{binCollection.PropertyLabel}, {binCollection.DaysToNextCollection},   {binCollection.Bins}";
        }

        throw new NotSupportedException();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


