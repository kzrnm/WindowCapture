using Kzrnm.WindowCapture.Images;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.WindowCapture.ViewModels
{
    public class ImageListViewModel : BindableBase
    {
        public ImageProvider ImageProvider { get; }
        public ImageListViewModel(ImageProvider imageProvider)
        {
            this.ImageProvider = imageProvider;
        }
    }
}
