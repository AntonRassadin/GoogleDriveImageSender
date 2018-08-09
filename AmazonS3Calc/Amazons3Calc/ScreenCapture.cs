using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Amazons3Calc
{
    class ScreenCapture
    {
        public void CaptureScreen(Visual target, double width, double height, out string path)
        {
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(Convert.ToInt32(width + 10), Convert.ToInt32(height + 10), 96, 96, PixelFormats.Default);
            renderTargetBitmap.Render(target);

            PngBitmapEncoder pngImage = new PngBitmapEncoder();
            pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            path = $"{Environment.CurrentDirectory}\\screen2{DateTime.Now.ToString("yyyyMMddHHmmss")}.png";

            using (Stream fileStream = System.IO.File.Create(path))
            {
                pngImage.Save(fileStream);
            }
        }
    }
}
