using Kzrnm.WindowCapture.Images;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Prism.Events;
using System;
using System.Collections.Generic;

namespace Kzrnm.WindowCapture.ViewModels
{
    public class ImageSettingsViewModel : ObservableObject
    {
        public static IReadOnlyCollection<ImageKind> ImageKinds { get; }
        static ImageSettingsViewModel()
        {
            ImageKinds = (ImageKind[])typeof(ImageKind).GetEnumValues();
        }

        private readonly IEventAggregator eventAggregator;
        public ImageProvider ImageProvider { get; }
        public ImageSettingsViewModel(IEventAggregator ea, ImageProvider imageProvider)
        {
            this.ImageProvider = imageProvider;
            this.eventAggregator = ea;
            this.SelectedImage = imageProvider.SelectedImage;

            SelectedImageChangedAction = t => SelectedImageChanged(t.oldImage, t.newImage);
            eventAggregator.GetEvent<SelectedImageChangedEvent>().Subscribe(SelectedImageChangedAction);
        }

        private readonly Action<(CaptureImage? oldImage, CaptureImage? newImage)> SelectedImageChangedAction;

        private CaptureImage? _SelectedImage;
        public CaptureImage? SelectedImage
        {
            get => _SelectedImage;
            private set => SetProperty(ref _SelectedImage, value);
        }

        public bool HasImage => SelectedImage is not null;

        public int Height
        {
            set
            {
                if (this.SelectedImage != null
                    && this.SelectedImage.ImageRatioSize.Height != value)
                {
                    this.SelectedImage.ImageRatioSize.Height = value;
                    this.OnPropertyChanged(nameof(Height));
                    this.OnPropertyChanged(nameof(HeightPercentage));
                }
            }
            get => this.SelectedImage?.ImageRatioSize.Height ?? 0;
        }
        public int Width
        {
            set
            {
                if (this.SelectedImage != null
                    && this.SelectedImage.ImageRatioSize.Width != value)
                {
                    this.SelectedImage.ImageRatioSize.Width = value;
                    this.OnPropertyChanged(nameof(Width));
                    this.OnPropertyChanged(nameof(WidthPercentage));
                }
            }
            get => this.SelectedImage?.ImageRatioSize.Width ?? 0;
        }

        public double WidthPercentage
        {
            set => UpdatePercentage(value);
            get => this.SelectedImage?.ImageRatioSize.WidthPercentage ?? 0;
        }
        public double HeightPercentage
        {
            set => UpdatePercentage(value);
            get => this.SelectedImage?.ImageRatioSize.HeightPercentage ?? 0;
        }

        private void UpdatePercentage(double value)
        {
            if (this.SelectedImage is { } image)
            {
                var imageRatioSize = image.ImageRatioSize;
                if (imageRatioSize.HeightPercentage != value)
                {
                    imageRatioSize.HeightPercentage = value;
                    this.OnPropertyChanged(nameof(Height));
                    this.OnPropertyChanged(nameof(HeightPercentage));
                }
                if (imageRatioSize.WidthPercentage != value)
                {
                    imageRatioSize.WidthPercentage = value;
                    this.OnPropertyChanged(nameof(Width));
                    this.OnPropertyChanged(nameof(WidthPercentage));
                }
            }
        }

        public ImageKind ImageKind
        {
            set
            {
                if (this.SelectedImage != null
                    && this.SelectedImage.ImageKind != value)
                {
                    this.OnPropertyChanged(nameof(ImageKind));
                    this.SelectedImage.ImageKind = value;
                }
            }
            get => this.SelectedImage?.ImageKind ?? ImageKind.JPG;
        }
        public bool IsSideCutMode
        {
            set
            {
                if (this.SelectedImage != null
                    && this.SelectedImage.IsSideCutMode != value)
                {
                    this.OnPropertyChanged(nameof(IsSideCutMode));
                    this.SelectedImage.IsSideCutMode = value;
                }
            }
            get => this.SelectedImage?.IsSideCutMode ?? false;
        }
        private void SelectedImageChanged(CaptureImage? oldImage, CaptureImage? newImage)
        {
            this.SelectedImage = newImage;
            if (oldImage == null)
            {
                if (newImage == null)
                    return;

                this.OnPropertyChanged(nameof(HasImage));
                this.OnPropertyChanged(nameof(Height));
                this.OnPropertyChanged(nameof(Width));
                this.OnPropertyChanged(nameof(HeightPercentage));
                this.OnPropertyChanged(nameof(WidthPercentage));
                this.OnPropertyChanged(nameof(ImageKind));
                this.OnPropertyChanged(nameof(IsSideCutMode));
                return;
            }

            if (newImage == null)
            {
                this.OnPropertyChanged(nameof(HasImage));
                //this.OnPropertyChanged(nameof(Height));
                //this.OnPropertyChanged(nameof(Width));
                //this.OnPropertyChanged(nameof(HeightPercentage));
                //this.OnPropertyChanged(nameof(WidthPercentage));
                //this.OnPropertyChanged(nameof(ImageKind));
                //this.OnPropertyChanged(nameof(IsSideCutMode));
                return;
            }

            if (oldImage.ImageRatioSize.Height != newImage.ImageRatioSize.Height)
                this.OnPropertyChanged(nameof(Height));
            if (oldImage.ImageRatioSize.Width != newImage.ImageRatioSize.Width)
                this.OnPropertyChanged(nameof(Width));
            if (oldImage.ImageRatioSize.HeightPercentage != newImage.ImageRatioSize.HeightPercentage)
                this.OnPropertyChanged(nameof(HeightPercentage));
            if (oldImage.ImageRatioSize.WidthPercentage != newImage.ImageRatioSize.WidthPercentage)
                this.OnPropertyChanged(nameof(WidthPercentage));
            if (oldImage.ImageKind != newImage.ImageKind)
                this.OnPropertyChanged(nameof(ImageKind));
            if (oldImage.IsSideCutMode != newImage.IsSideCutMode)
                this.OnPropertyChanged(nameof(IsSideCutMode));
        }

    }
}
