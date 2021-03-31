using GongSolutions.Wpf.DragDrop;
using Kzrnm.WindowCapture.Images;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CapApp.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel(ImageProvider imageProvider)
        {
            this.ImageProvider = imageProvider;
        }
        public ImageProvider ImageProvider { get; }


        private static readonly Random rnd = new();
        private static BitmapSource CreateRandom()
        {
            PixelFormat pf = PixelFormats.Rgb24;
            var width = rnd.Next(1, 800);
            var height = rnd.Next(1, 800);
            int stride = (width * pf.BitsPerPixel + 7) / 8;
            var bytes = new byte[stride * height];
            rnd.NextBytes(bytes);
            return BitmapSource.Create(width, height, 96, 96, pf, null, bytes, stride);
        }
        private RelayCommand? _AddCommand;
        public RelayCommand AddCommand => _AddCommand ??= new RelayCommand(() => ImageProvider.AddImage(CreateRandom()));
        private RelayCommand? _ClearCommand;
        public RelayCommand ClearCommand => _ClearCommand ??= new RelayCommand(() =>
        {
            ImageProvider.Images.Clear();
        });
    }
}
