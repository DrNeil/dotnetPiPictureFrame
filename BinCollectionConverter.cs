using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace dotnetPiPictureFrame
{
    internal class BinCollectionConverter: IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            if (value is Bins.BinCollection binCollection)
            {
                return $"In {binCollection.DaysToNextCollection} day(s), {binCollection.Bins}";
            }

            throw new NotSupportedException();
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
