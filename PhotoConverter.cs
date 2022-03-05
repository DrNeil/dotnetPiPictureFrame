using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using System;
using System.Globalization;
using System.IO;

namespace dotnetPiPictureFrame
{
	internal class PhotoConverter : IValueConverter
	{
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			if (value == null)
				return null;

			if (value is string filepath
			 && File.Exists(filepath))
			{
				return new Bitmap(filepath);
			}

			throw new NotSupportedException();
		}

		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
