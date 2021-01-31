using Prism.Mvvm;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kzrnm.WindowCapture.Images
{
    public class CaptureImage : BindableBase
    {
        public ImageRatioSize ImageRatioSize { get; }

        private ImageKind _ImageKind;
        public ImageKind ImageKind
        {
            set => SetProperty(ref _ImageKind, value, UpdateTransformedImage);
            get => _ImageKind;
        }

        private bool _IsSideCutMode;
        public bool IsSideCutMode
        {
            set => SetProperty(ref _IsSideCutMode, value, UpdateTransformedImage);
            get => _IsSideCutMode;
        }

        private void UpdateTransformedImage()
        {
            var bitmap = ImageSource;
            if (IsSideCutMode)
            {
                bitmap =
                   new CroppedBitmap(ImageSource,
                   new Int32Rect(ImageSource.PixelWidth / 8, 0, ImageSource.PixelWidth * 3 / 4, ImageSource.PixelHeight));
                bitmap.Freeze();
            }
            if (this.ImageRatioSize.WidthPercentage != 100
                || this.ImageRatioSize.HeightPercentage != 100)
            {
                bitmap =
                    new TransformedBitmap(bitmap,
                    new ScaleTransform(this.ImageRatioSize.WidthPercentage / 100,
                    this.ImageRatioSize.HeightPercentage / 100));
                bitmap.Freeze();
            }
            this.TransformedImage = bitmap;
        }

        private BitmapSource _TransformedImage;
        public BitmapSource TransformedImage
        {
            set => SetProperty(ref _TransformedImage, value);
            get => _TransformedImage;
        }

        public BitmapSource ImageSource { get; }
        public string? SourcePath { get; }
        private ImageKind OrigKind { get; }

        public static int DefaultJpegQualityLevel { set; get; } = 100;
        public int JpegQualityLevel { get; set; } = DefaultJpegQualityLevel;

        [MemberNotNullWhen(returnValue: true, member: nameof(SourcePath))]
        public bool CanUseFileStream
            => this.SourcePath != null
                && this.ImageRatioSize.IsNotChanged
                && !IsSideCutMode
                && this.ImageKind == this.OrigKind;

        public CaptureImage(BitmapSource source) : this(source, null) { }
        public CaptureImage(BitmapSource source, string? sourcePath)
        {
            this.ImageSource = source;
            this._TransformedImage = source;

            this.SourcePath = sourcePath;
            this.ImageRatioSize = new ImageRatioSize(source);
            this.IsSideCutMode = false;
            if (sourcePath is null)
            {
                this.ImageKind = ImageKind.JPG;
            }
            else
            {
                this.ImageKind = this.OrigKind = Regex.IsMatch(Path.GetExtension(sourcePath), @"\.jpe?g", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)
                    ? ImageKind.JPG
                    : ImageKind.PNG;
            }

            this.ImageRatioSize.PropertyChanged += (_, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(ImageRatioSize.HeightPercentage):
                    case nameof(ImageRatioSize.WidthPercentage):
                        this.UpdateTransformedImage();
                        break;
                }
            };
        }

        private BitmapEncoder GetEncoder()
            => this.ImageKind switch
            {
                ImageKind.JPG => new JpegBitmapEncoder { QualityLevel = this.JpegQualityLevel },
                ImageKind.PNG => new PngBitmapEncoder { Interlace = PngInterlaceOption.Off },
                _ => throw new InvalidOperationException($"invalid {nameof(ImageKind)}"),
            };

        private byte[] ToStreamImpl()
        {
            BitmapEncoder encoder = this.GetEncoder();
            encoder.Frames.Add(BitmapFrame.Create(this.TransformedImage));

            using var ms = new MemoryStream();
            encoder.Save(ms);
            return ms.ToArray();
        }

        public byte[] ToByteArray() => CanUseFileStream ? File.ReadAllBytes(this.SourcePath) : ToStreamImpl();
    }
}
