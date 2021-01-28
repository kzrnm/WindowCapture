using Kzrnm.WindowCapture.Images;
using Kzrnm.WindowCapture.Windows;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Windows;

namespace Kzrnm.WindowCapture.ViewModels
{
    public class ImagePreviewWindowViewModel : BindableBase
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IClipboardManager clipboard;
        public ImageProvider ImageProvider { get; }
        public ImagePreviewWindowViewModel(IEventAggregator ea, IClipboardManager clipboard, ImageProvider imageProvider)
        {
            this.ImageProvider = imageProvider;
            this.eventAggregator = ea;
            this.clipboard = clipboard;

            SelectedImageChanged = t => this.SelectedImage = t.newImage;
            eventAggregator.GetEvent<SelectedImageChangedEvent>().Subscribe(SelectedImageChanged);
        }

        private readonly Action<(CaptureImage? oldImage, CaptureImage? newImage)> SelectedImageChanged;

        private DelegateCommand<CaptureImage?>? _CopyToClipboardCommand;
        public DelegateCommand<CaptureImage?> CopyToClipboardCommand
            => _CopyToClipboardCommand ??= new DelegateCommand<CaptureImage?>(CopySelectedImageToClipboard);
        private void CopySelectedImageToClipboard(CaptureImage? obj)
        {
            if (obj is { } image)
                this.clipboard.SetImage(image.TransformedImage);
        }


        public void UpdateCanClipboardCommand() => _PasteImageFromClipboardCommand?.RaiseCanExecuteChanged();
        private DelegateCommand? _PasteImageFromClipboardCommand;
        public DelegateCommand PasteImageFromClipboardCommand
            => _PasteImageFromClipboardCommand ??= new DelegateCommand(PasteImageFromClipboard, clipboard.ContainsImage);
        private void PasteImageFromClipboard()
        {
            if (this.clipboard.GetImage() is { } image)
                this.ImageProvider.AddImage(image);
        }


        private CaptureImage? _SelectedImage;
        public CaptureImage? SelectedImage
        {
            get => _SelectedImage;
            set => SetProperty(ref _SelectedImage, value, () => Visibility = _SelectedImage is null ? Visibility.Collapsed : Visibility.Visible);
        }

        private Visibility _Visibility = Visibility.Collapsed;
        public Visibility Visibility
        {
            get => _Visibility;
            set => SetProperty(ref _Visibility, value);
        }



        private DelegateCommand? _ClearImageCommand;
        public DelegateCommand ClearImageCommand
            => _ClearImageCommand ??= new DelegateCommand(ClearImage);
        public void ClearImage()
        {
            this.ImageProvider.Images.Clear();
        }
    }
}
