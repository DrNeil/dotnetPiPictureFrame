using System;
using System.Globalization;
using System.Linq;
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

            if (value is Bins.BinInfo binInfo)
            {
                if (binInfo.Status == "OK")
                {
                    if (binInfo.DaysToNextCollection == 0)
                    {
                        return "Today";
                    }
                    else if (binInfo.DaysToNextCollection == 1)
                    {
                        return "Tomorrow";
                    }
                    else if (binInfo.DaysToNextCollection > 1)
                    {
                        return $"In {binInfo.DaysToNextCollection} days";
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

            if (value is Bins.BinInfo binInfo)
            {
                var binNumber = int.Parse(binString);
                if (binInfo.Bins != null
                    && binInfo.Bins.Count() > binNumber)
                {
                    var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
                    var bitmap = new Bitmap(assets?.Open(new Uri($"avares://dotnetPiPictureFrame/Assets/{binInfo.Bins.ElementAt(binNumber)}.png")));
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
