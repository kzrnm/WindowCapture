using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kzrnm.WindowCapture
{
    public static class TestUtil
    {
        public static BitmapSource DummyBitmapSource(int width, int height)
        {
            var stride = (width * PixelFormats.Indexed1.BitsPerPixel + 7) / 8;
            return BitmapSource.Create(
            width, height, 96, 96,
            PixelFormats.Indexed1,
            new BitmapPalette(new[] { Colors.Transparent }),
            new byte[stride * width], stride);
        }
    }
}
