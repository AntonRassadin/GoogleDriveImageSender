using System.Threading.Tasks;
using System.Windows;

namespace GoogleDriveImageSender
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GoogleDriveFileTransfer fileTransfer;
        ScreenCapture screenCapture;

        public MainWindow()
        {
            InitializeComponent();
            fileTransfer = new GoogleDriveFileTransfer();
            screenCapture = new ScreenCapture();
        }

        private void ExportToAWSButton_Click(object sender, RoutedEventArgs e)
        {
            screenCapture.CaptureScreen(outputBox, outputBox.ActualWidth, outputBox.ActualHeight, out string path);
            AsyncUploadImage(path, "image/png");
        }

        async void AsyncUploadImage(string path, string mimeType)
        {
           outputInfo.Content = await UploadFileAndReport(path, mimeType);
        }

        private Task<string> UploadFileAndReport(string path, string mimeType)
        {
            return Task.Run(() => fileTransfer.UploadFile(path, "image/png"));
        }
    }
}
