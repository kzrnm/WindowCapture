using Kzrnm.WindowCapture.ViewModels;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
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
            this.DataContext = Ioc.Default.GetService<ImageListViewModel>();
        }
    }
}
