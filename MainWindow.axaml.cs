using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
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
        
        Image frameImage;
        Bitmap? frameBmp;
        public MainWindow()
        {
            InitializeComponent();
            photoImage = this.FindControl<Image>("PhotoImage");
            frameImage = this.FindControl<Image>("FrameImage");
            DataContext = viewModel;
            Task.Run(async () => await UpdateGUI());
            Task.Run(async () => await UpdateFrameImage());
        }

        async Task UpdateGUI()
        {
            var files = (Directory.GetFiles(Config.PhotosFolder, "*.jpg").Union(Directory.GetFiles(Config.PhotosFolder, "*.png"))).ToArray();
            int currentPhoto = 0;
            bool photoDisplayed = false;

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
                        Dispatcher.UIThread.Post(() => photoImage?.Classes.Add("exiting"));
                        Dispatcher.UIThread.Post(() => photoImage?.Classes.Remove("entering"));
                        updatePhotoPeriod = 1;
                        photoDisplayed = false;
                    }
                    else if (files.Length > 0)
                    {
                        viewModel.PhotoPath = files[currentPhoto];
                        currentPhoto++;
                        if (currentPhoto >= files.Length)
                        {
                            currentPhoto = 0;
                        }
                        Dispatcher.UIThread.Post(() => photoImage?.Classes.Add("entering"));
                        Dispatcher.UIThread.Post(() => photoImage?.Classes.Remove("exiting"));
                        updatePhotoPeriod = 10;
                        photoDisplayed = true;
                    }
                    photoPeriodSeconds = 0;
                }
                photoPeriodSeconds++;
                await Task.Delay(1000);
            }
        }

        async Task UpdateFrameImage()
        {
            if (frameImage != null)
            {
                HttpClient client = new HttpClient();

                using HttpResponseMessage response = await client.GetAsync(Config.VideoUrl, HttpCompletionOption.ResponseHeadersRead);
                if (response.IsSuccessStatusCode)
                {
                    using HttpContent content = response.Content;
                    using var stream = await content.ReadAsStreamAsync();

                    byte[] buffer = new byte[4096];
                    var lengthMarker = "Content-Length:";
                    var endMarker = "\r\n\r\n";

                    while (true)
                    {
                        try
                        {
                            Array.Fill<byte>(buffer, 0, 0, buffer.Length);
                            int len = await stream.ReadAsync(buffer, 0, buffer.Length);
                            var header = System.Text.Encoding.Default.GetString(buffer);

                            var lengthStart = header.IndexOf(lengthMarker) + lengthMarker.Length;
                            var lengthEnd = header.IndexOf(endMarker);
                            if (lengthEnd > lengthStart)
                            {
                                var lengthString = header.Substring(lengthStart, lengthEnd - lengthStart);

                                int frameSize = int.Parse(lengthString);
                                byte[] frameBuffer = new byte[frameSize];

                                int totalBytesCopied = (int)len - (lengthEnd + endMarker.Length);
                                if (totalBytesCopied > 0)
                                {
                                    Array.Copy(buffer, lengthEnd + endMarker.Length, frameBuffer, 0, totalBytesCopied);
                                }

                                while (totalBytesCopied < frameSize)
                                {
                                    totalBytesCopied += await stream.ReadAsync(frameBuffer, totalBytesCopied, frameBuffer.Length - totalBytesCopied);
                                    await Task.Yield();
                                }
                                using MemoryStream ms = new(frameBuffer);
                                frameBmp = new Bitmap(ms);

                                await Dispatcher.UIThread.InvokeAsync(() => { frameImage.Source = frameBmp; });
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }
            }
        }

    }
}