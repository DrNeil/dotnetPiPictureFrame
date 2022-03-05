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
        int updatePhotoPeriod = 10;
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
            viewModel.PhotoPath = files[currentPhoto];
            bool photoDisplayed = true;

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
                    if (photoDisplayed)
                    {
                        Dispatcher.UIThread.Post(() => photoImage.Classes.Add("exiting"));
                        Dispatcher.UIThread.Post(() => photoImage.Classes.Remove("entering"));
                        updatePhotoPeriod = 1;
                        photoDisplayed = false;
                    }
                    else
                    {
                        currentPhoto++;
                        if (currentPhoto >= files.Length)
                        {
                            currentPhoto = 0;
                        }
                        viewModel.PhotoPath = files[currentPhoto];
                        Dispatcher.UIThread.Post(() => photoImage.Classes.Add("entering"));
                        Dispatcher.UIThread.Post(() => photoImage.Classes.Remove("exiting"));
                        updatePhotoPeriod = 10;
                        photoDisplayed = true;
                    }
                    photoPeriodSeconds = 0;
                }
                photoPeriodSeconds++;
                await Task.Delay(1000);
            }
        }
    }
}