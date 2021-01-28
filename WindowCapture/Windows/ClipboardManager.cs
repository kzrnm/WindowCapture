using System.Windows;
using System.Windows.Media.Imaging;

namespace Kzrnm.WindowCapture.Windows
{
    public interface IClipboardManager
    {
        public void SetImage(BitmapSource image);
        public BitmapSource GetImage();
        public bool ContainsImage();
    }
    public class ClipboardManager : IClipboardManager
    {
        public ClipboardManager() { }
        public void SetImage(BitmapSource image) => Clipboard.SetImage(image);
        public BitmapSource GetImage() => Clipboard.GetImage();
        public bool ContainsImage() => Clipboard.ContainsImage();
    }
}
