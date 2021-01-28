using Prism.Mvvm;
using System.Windows;
using System.Windows.Controls;

namespace Kzrnm.WindowCapture.Views
{
    public class ImageSettings : Control
    {
        static ImageSettings()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageSettings), new FrameworkPropertyMetadata(typeof(ImageSettings)));
        }

        public ImageSettings()
        {
            ViewModelLocator.SetAutoWireViewModel(this, true);
        }
    }
}
