using Avalonia.Controls;
using Weather.NET;
using Weather.NET.Enums;

namespace dotnetPiPictureFrame
{
    public partial class MainWindow : Window
    {
        FrameViewModel viewModel = new FrameViewModel();
        WeatherClient client = new WeatherClient(Keys.OpenWeather);
        
        public MainWindow()
        {
            viewModel.Time = System.DateTime.Now;
            viewModel.CurrentWeather = client.GetCurrentWeather(cityName: Keys.WeatherCity, measurement: Measurement.Metric);
            viewModel.PhotoPath = @"\\Creative\Photos\PicFrame1\LachSonnKrisNeil.png";
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}