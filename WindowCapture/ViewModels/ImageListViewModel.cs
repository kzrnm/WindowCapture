using GongSolutions.Wpf.DragDrop;
using Kzrnm.WindowCapture.Images;
using Kzrnm.WindowCapture.Windows;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace Kzrnm.WindowCapture.ViewModels
{
    public class ImageListViewModel : ObservableRecipient, IRecipient<SelectedImageChangedMessage>
    {
        private readonly IClipboardManager clipboard;
        public ImageProvider ImageProvider { get; }
        public ImageListViewModel(WeakReferenceMessenger messenger, IClipboardManager clipboardManager, ImageProvider imageProvider)
            : base(messenger)
        {
            this.ImageProvider = imageProvider;
            this.clipboard = clipboardManager;
            this.DropHandler = new ImageDropTarget(imageProvider);
            IsActive = true;
        }

        void IRecipient<SelectedImageChangedMessage>.Receive(SelectedImageChangedMessage message)
        {
            _RemoveSelectedImageCommand?.NotifyCanExecuteChanged();
        }

        private RelayCommand? _RemoveSelectedImageCommand;
        public RelayCommand RemoveSelectedImageCommand
            => _RemoveSelectedImageCommand ??= new RelayCommand(() => ImageProvider.Images.RemoveSelectedItem(), () => ImageProvider.SelectedImageIndex >= 0);


        public void UpdateCanClipboardCommand() => _InsertImageFromClipboardCommand?.NotifyCanExecuteChanged();

        private RelayCommand? _InsertImageFromClipboardCommand;
        public RelayCommand InsertImageFromClipboardCommand
            => _InsertImageFromClipboardCommand ??= new RelayCommand(InsertImageFromClipboard, clipboard.ContainsImage);
        private void InsertImageFromClipboard()
        {
            if (clipboard.GetImage() is { } image)
                ImageProvider.InsertImage(ImageProvider.SelectedImageIndex + 1, image);
        }


        private RelayCommand<CaptureImage>? _CopyToClipboardCommand;
        public RelayCommand<CaptureImage> CopyToClipboardCommand
            => _CopyToClipboardCommand ??= new RelayCommand<CaptureImage>(CopySelectedImageToClipboard);

        private void CopySelectedImageToClipboard(CaptureImage? obj)
        {
            if (obj is { } image)
                this.clipboard.SetImage(image.TransformedImage);
        }

        public IDropTarget DropHandler { get; }
    }
}
