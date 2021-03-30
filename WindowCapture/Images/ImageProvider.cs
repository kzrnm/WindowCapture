using Kzrnm.WindowCapture.Mvvm;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using System.Windows.Media.Imaging;

namespace Kzrnm.WindowCapture.Images
{
    public class ImageProvider : ObservableObject
    {
        public ImageProvider(WeakReferenceMessenger messenger) : this(messenger, new SelectorObservableCollection<CaptureImage>()) { }
        protected ImageProvider(WeakReferenceMessenger messenger, SelectorObservableCollection<CaptureImage> images)
        {
            this._Images = images;
            this.Messenger = messenger;
            images.SelectedIndexChanged += (_, _) =>
            {
                SelectedImageIndex = this._Images.SelectedIndex;
                SelectedImage = this._Images.SelectedItem;
            };
        }

        protected WeakReferenceMessenger Messenger { get; }
        protected readonly SelectorObservableCollection<CaptureImage> _Images;
        public SelectorObservableCollection<CaptureImage> Images => this._Images;

        private int _SelectedImageIndex = -1;
        public int SelectedImageIndex
        {
            set
            {
                if (this.SetProperty(ref _SelectedImageIndex, value))
                {
                    this._Images.SelectedIndex = value;
                }
            }
            get => _SelectedImageIndex;
        }
        private CaptureImage? _SelectedImage;
        public CaptureImage? SelectedImage
        {
            private set
            {
                var current = _SelectedImage;
                if (this.SetProperty(ref _SelectedImage, value))
                {
                    if (value != null)
                        lastSelectedOption.Load(value);
                    Messenger.Send(new SelectedImageChangedMessage(this, nameof(SelectedImage), current, value));
                }
            }
            get => _SelectedImage;
        }

        private readonly ImageOption lastSelectedOption = new();

        private void ApplyLastOption(CaptureImage image)
        {
            if (this.SelectedImage is { } selectedImage)
            {
                image.ImageRatioSize.WidthPercentage = selectedImage.ImageRatioSize.WidthPercentage;
                image.ImageRatioSize.HeightPercentage = selectedImage.ImageRatioSize.HeightPercentage;
                image.ImageKind = selectedImage.ImageKind;
                image.IsSideCutMode = selectedImage.IsSideCutMode;
            }
            else
            {
                image.ImageRatioSize.WidthPercentage = lastSelectedOption.WidthPercentage;
                image.ImageRatioSize.HeightPercentage = lastSelectedOption.HeightPercentage;
                image.ImageKind = lastSelectedOption.ImageKind;
                image.IsSideCutMode = lastSelectedOption.IsSideCutMode;
            }
        }

        public virtual bool CanAddImage => true;

        public void AddImage(BitmapSource bmp)
        {
            if (!CanAddImage) return;
            bmp.Freeze();
            var image = new CaptureImage(bmp);
            ApplyLastOption(image);
            this.Images.Add(image);
        }
        public bool TryAddImageFromFile(string filePath)
        {
            if (!CanAddImage) return false;
            var image = CaptureImageUtils.GetImageFromFile(filePath);
            if (image == null) return false;
            ApplyLastOption(image);
            this.Images.Add(image);
            return true;
        }
        public void InsertImage(int index, BitmapSource bmp)
        {
            if (!CanAddImage) return;
            bmp.Freeze();
            var image = new CaptureImage(bmp);
            ApplyLastOption(image);
            this.Images.Insert(index, image);
        }
        public bool TryInsertImageFromFile(int index, string filePath)
        {
            if (!CanAddImage) return false;
            var image = CaptureImageUtils.GetImageFromFile(filePath);
            if (image == null) return false;
            ApplyLastOption(image);
            this.Images.Insert(index, image);
            return true;
        }


        private class ImageOption
        {
            public ImageOption() { }
            public ImageOption(CaptureImage image) => Load(image);

            public void Load(CaptureImage image)
            {
                this.WidthPercentage = image.ImageRatioSize.WidthPercentage;
                this.HeightPercentage = image.ImageRatioSize.HeightPercentage;
                this.ImageKind = image.ImageKind;
                this.IsSideCutMode = image.IsSideCutMode;
            }

            public double WidthPercentage { set; get; } = 100.0;
            public double HeightPercentage { set; get; } = 100.0;
            public ImageKind ImageKind { set; get; } = ImageKind.JPG;
            public bool IsSideCutMode { set; get; } = false;
        }
    }
}
