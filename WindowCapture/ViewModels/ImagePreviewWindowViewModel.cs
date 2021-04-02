using Kzrnm.WindowCapture.Images;
using Kzrnm.WindowCapture.Windows;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System.Windows;

namespace Kzrnm.WindowCapture.ViewModels
{
    public class ImagePreviewWindowViewModel : ObservableRecipient, IRecipient<SelectedImageChangedMessage>
    {
        private readonly IClipboardManager clipboard;
        public ImageProvider ImageProvider { get; }
        public ImagePreviewWindowViewModel(ICaptureImageService captureImageService, IClipboardManager clipboard, ImageProvider imageProvider)
            : this(WeakReferenceMessenger.Default, captureImageService, clipboard, imageProvider)
        { }
        public ImagePreviewWindowViewModel(IMessenger messenger, ICaptureImageService captureImageService, IClipboardManager clipboard, ImageProvider imageProvider)
            : base(messenger)
        {
            this.DropHandler = new ImageDropTarget(captureImageService, imageProvider, false);
            this.ImageProvider = imageProvider;
            this.clipboard = clipboard;
            IsActive = true;
        }

        void IRecipient<SelectedImageChangedMessage>.Receive(SelectedImageChangedMessage message)
        {
            this.SelectedImage = message.NewValue;
        }

        public ImageDropTarget DropHandler { get; }

        private RelayCommand<CaptureImage?>? _CopyToClipboardCommand;
        public RelayCommand<CaptureImage?> CopyToClipboardCommand
            => _CopyToClipboardCommand ??= new RelayCommand<CaptureImage?>(CopySelectedImageToClipboard);
        private void CopySelectedImageToClipboard(CaptureImage? obj)
        {
            if (obj is { } image)
                this.clipboard.SetImage(image.TransformedImage);
        }


        public void UpdateCanClipboardCommand() => _PasteImageFromClipboardCommand?.NotifyCanExecuteChanged();
        private RelayCommand? _PasteImageFromClipboardCommand;
        public RelayCommand PasteImageFromClipboardCommand
            => _PasteImageFromClipboardCommand ??= new RelayCommand(PasteImageFromClipboard, clipboard.ContainsImage);
        private void PasteImageFromClipboard()
        {
            if (this.clipboard.GetImage() is { } image)
                this.ImageProvider.AddImage(image);
        }


        private CaptureImage? _SelectedImage;
        public CaptureImage? SelectedImage
        {
            get => _SelectedImage;
            set
            {
                if (SetProperty(ref _SelectedImage, value))
                    Visibility = _SelectedImage is null ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private Visibility _Visibility = Visibility.Collapsed;
        public Visibility Visibility
        {
            get => _Visibility;
            set => SetProperty(ref _Visibility, value);
        }



        private RelayCommand? _ClearImageCommand;
        public RelayCommand ClearImageCommand
            => _ClearImageCommand ??= new RelayCommand(ClearImage);
        public void ClearImage()
        {
            this.ImageProvider.Images.Clear();
        }
    }
}
