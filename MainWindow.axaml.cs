using Avalonia.Controls;

namespace dotnetPiPictureFrame
{
    public partial class MainWindow : Window
    {
        FrameViewModel viewModel = new FrameViewModel();
        public MainWindow()
        {
            viewModel.Time = System.DateTime.Now;
            viewModel.PhotoPath = @"\\Creative\Photos\PicFrame1\LachSonnKrisNeil.png";
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}