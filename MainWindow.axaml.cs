using Avalonia.Controls;
using Avalonia.Threading;
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
        int updatePhotoPeriod = 1;
        int photoPeriodSeconds = 0;
        Image? photoImage;

        public MainWindow()
        {
            InitializeComponent();
            photoImage = this.FindControl<Image>("PhotoImage");
            DataContext = viewModel;
            Task.Run(async () => await UpdateGUI());
        }

        async Task UpdateGUI()
        {
            var files = (Directory.GetFiles(Config.PhotosFolder, "*.jpg").Union(Directory.GetFiles(Config.PhotosFolder, "*.png"))).ToArray();
            int currentPhoto = 0;
            
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
                    if (files.Length > 0)
                    {
                        viewModel.PhotoPath = files[currentPhoto];
                        currentPhoto++;
                        if (currentPhoto >= files.Length)
                        {
                            currentPhoto = 0;
                        }
                        updatePhotoPeriod = 10;
                    }
                    photoPeriodSeconds = 0;
                }
                photoPeriodSeconds++;
                await Task.Delay(1000);
            }
        }
    }
}