using Avalonia.Controls;
using System;
using System.Threading.Tasks;
using Weather.NET;
using Weather.NET.Enums;

namespace dotnetPiPictureFrame
{
    public partial class MainWindow : Window
    {
        FrameViewModel viewModel = new FrameViewModel();
        WeatherClient client = new WeatherClient(Config.OpenWeather);
        const int checkWeatherPeriod = 60;
        int currentPeriodSeconds = 0;

        public MainWindow()
        {
            viewModel.PhotoPath = Config.PhotosFolder;
            InitializeComponent();
            DataContext = viewModel;
            Task.Run(async () => await UpdateGUI());
        }

        private async Task UpdateGUI()
        {
            viewModel.CurrentWeather = client.GetCurrentWeather(cityName: Config.WeatherCity, measurement: Measurement.Metric);
            while (true)
            {
                viewModel.Time = DateTime.Now;
                
                if (currentPeriodSeconds > checkWeatherPeriod)
                {
                    viewModel.CurrentWeather = client.GetCurrentWeather(cityName: Config.WeatherCity, measurement: Measurement.Metric);
                    currentPeriodSeconds = 0;
                }
                currentPeriodSeconds++;
                await Task.Delay(1000);
            }
        }
    }
}