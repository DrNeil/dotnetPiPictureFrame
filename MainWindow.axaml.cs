using Avalonia.Controls;
using Weather.NET;

namespace dotnetPiPictureFrame
{
    public partial class MainWindow : Window
    {
        FrameViewModel viewModel = new FrameViewModel();
        WeatherClient client = new WeatherClient(Keys.OpenWeather);
        
        public MainWindow()
        {
            viewModel.Time = System.DateTime.Now;
            viewModel.PhotoPath = @"\\Creative\Photos\PicFrame1\LachSonnKrisNeil.png";
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}