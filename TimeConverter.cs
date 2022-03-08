using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace dotnetPiPictureFrame
{
	internal class TimeConverter : IValueConverter
	{
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			if (value == null)
				return null;

			if (value is DateTime time)
			{
				return time.ToString("dd MMM yy HH:mm");
			}

			throw new NotSupportedException();
		}

		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
