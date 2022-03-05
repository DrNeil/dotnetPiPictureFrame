using Avalonia.Controls;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Weather.NET;
using Weather.NET.Enums;

namespace dotnetPiPictureFrame
{
    public partial class MainWindow : Window
    {
        FrameViewModel viewModel = new FrameViewModel();
        WeatherClient client = new WeatherClient(Config.OpenWeather);
        const int checkWeatherPeriod = 60*30;
        int weatherPeriodSeconds = 0;
        const int updatePhotoPeriod = 10;
        int photoPeriodSeconds = 0;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel;
            Task.Run(async () => await UpdateGUI());
        }

        private async Task UpdateGUI()
        {
            var files = (Directory.GetFiles(Config.PhotosFolder, "*.jpg").Union(Directory.GetFiles(Config.PhotosFolder, "*.png"))).ToArray();
            int currentFile = 0;
            viewModel.PhotoPath = files[currentFile];
            viewModel.CurrentWeather = client.GetCurrentWeather(cityName: Config.WeatherCity, measurement: Measurement.Metric);
            while (true)
            {
                viewModel.Time = DateTime.Now;
                
                if (weatherPeriodSeconds > checkWeatherPeriod)
                {
                    viewModel.CurrentWeather = client.GetCurrentWeather(cityName: Config.WeatherCity, measurement: Measurement.Metric);
                    weatherPeriodSeconds = 0;
                }
                weatherPeriodSeconds++;

                if (photoPeriodSeconds > updatePhotoPeriod)
                {
                    currentFile++;
                    if (currentFile >= files.Length)
                    {
                        currentFile = 0;
                    }
                    viewModel.PhotoPath = files[currentFile];
                    photoPeriodSeconds = 0;
                }
                photoPeriodSeconds++;
                await Task.Delay(1000);
            }
        }
    }
}