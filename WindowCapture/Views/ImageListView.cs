using Prism.Mvvm;
using System.Windows;
using System.Windows.Controls;

namespace Kzrnm.WindowCapture.Views
{
    public class ImageListView : Control
    {
        static ImageListView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageListView), new FrameworkPropertyMetadata(typeof(ImageListView)));
        }
        public ImageListView()
        {
            ViewModelLocator.SetAutoWireViewModel(this, true);
        }
    }
}
