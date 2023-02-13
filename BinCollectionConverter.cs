using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

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
                if (binCollection.Bins != null)
                {
                    if (binCollection.DaysToNextCollection == 0)
                    {
                        return "Today";
                    }
                    else if (binCollection.DaysToNextCollection == 1)
                    {
                        return "Tomorrow";
                    }
                    else if (binCollection.DaysToNextCollection > 1)
                    {
                        return $"In {binCollection.DaysToNextCollection} days";
                    }
                    else
                    {
                        return "Cannot find bins to collect";
                    }
                    
                }
                else
                {
                    return "Cannot find bins to collect";
                }
            }

            throw new NotSupportedException();
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class BinImageConverter: IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return null;
            var binString = parameter.ToString();
            if (binString == null)
                return null;

            if (value is Bins.BinCollection binCollection)
            {
                var binNumber = int.Parse(binString);
                if (binCollection.Bins != null
                    && binCollection.Bins.Length > binNumber)
                {
                    var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
                    var bitmap = new Bitmap(assets?.Open(new Uri($"avares://dotnetPiPictureFrame/Assets/{binCollection.Bins[binNumber]}.png")));
                    return bitmap;//new Bitmap(filepath);
                }
                else
                {
                    return null;
                }
            }

            throw new NotSupportedException();
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
