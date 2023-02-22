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
        const int checkPeriod = 60*30;
        int periodSeconds = 0;
        int updatePhotoPeriod = 1;
        int photoPeriodSeconds = 0;
        Image? photoImage;
        string[] photoFiles;

        public MainWindow()
        {
            InitializeComponent();
            photoImage = this.FindControl<Image>("PhotoImage");
            DataContext = viewModel;
            Task.Run(async () => await UpdateGUI());
        }

        void UpdatePhotos()
        {
            if (Directory.Exists(Config.PhotosFolder))
            {
                photoFiles = (Directory.GetFiles(Config.PhotosFolder, "*.jpg").Union(Directory.GetFiles(Config.PhotosFolder, "*.png"))).ToArray();
            }
            else
            {
                photoFiles = new string[0];
            }
        }

        async Task UpdateWeather()
        {
            try
            {
                viewModel.CurrentWeather = await client.GetCurrentWeatherAsync(cityName: Config.WeatherCity, measurement: Measurement.Metric);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        async Task UpdateGUI()
        {
            int currentPhoto = 0;
            var bins = new Bins.BinCollection();
            viewModel.BinInfo = await bins.GetBins(Config.Address);
            _ = Task.Run(() => UpdatePhotos());
            _ = Task.Run(() => UpdateWeather());
            while (true)
            {
                viewModel.Time = DateTime.Now;
                
                if (periodSeconds > checkPeriod)
                {
                    _ = Task.Run(() => UpdatePhotos());
                    _ = Task.Run(() => UpdateWeather());
                    periodSeconds = 0;
                }
                periodSeconds++;

                if (photoPeriodSeconds > updatePhotoPeriod)
                {
                    if (photoFiles.Length > 0)
                    {
                        viewModel.PhotoPath = photoFiles[currentPhoto];
                        currentPhoto++;
                        if (currentPhoto >= photoFiles.Length)
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