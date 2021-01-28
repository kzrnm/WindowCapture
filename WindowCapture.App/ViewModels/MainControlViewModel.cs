using Kzrnm.WindowCapture.Images;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CapApp.ViewModels
{
    public class MainControlViewModel : BindableBase
    {
        public ImageProvider ImageProvider { get; }

        private static readonly Random rnd = new Random();
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
        private DelegateCommand? _AddCommand;
        public DelegateCommand AddCommand => _AddCommand ??= new DelegateCommand(() => ImageProvider.AddImage(CreateRandom()));
        private DelegateCommand? _ClearCommand;
        public DelegateCommand ClearCommand => _ClearCommand ??= new DelegateCommand(ImageProvider.Images.Clear);
        public MainControlViewModel(ImageProvider imageProvider)
        {
            ImageProvider = imageProvider;
        }
    }
}
