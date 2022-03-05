using Avalonia.Data.Converters;
using System;
using System.Globalization;
using Weather.NET.Models.WeatherModel;

namespace dotnetPiPictureFrame
{
	internal class WeatherConverter : IValueConverter
	{
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			if (value == null)
				return null;

			if (value is WeatherModel weather)
			{
				return $"{weather.CityName}, {weather.Weather[0].Title},   {weather.Main.Temperature}\u2103";
			}

			throw new NotSupportedException();
		}

		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
