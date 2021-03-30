using GongSolutions.Wpf.DragDrop;
using Kzrnm.WindowCapture.Images;
using Kzrnm.WindowCapture.Windows;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using Prism.Commands;

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
            _RemoveSelectedImageCommand?.RaiseCanExecuteChanged();
        }

        private DelegateCommand? _RemoveSelectedImageCommand;
        public DelegateCommand RemoveSelectedImageCommand
            => _RemoveSelectedImageCommand ??= new DelegateCommand(() => ImageProvider.Images.RemoveSelectedItem(), () => ImageProvider.SelectedImageIndex >= 0);


        public void UpdateCanClipboardCommand() => _InsertImageFromClipboardCommand?.RaiseCanExecuteChanged();

        private DelegateCommand? _InsertImageFromClipboardCommand;
        public DelegateCommand InsertImageFromClipboardCommand
            => _InsertImageFromClipboardCommand ??= new DelegateCommand(InsertImageFromClipboard, clipboard.ContainsImage);
        private void InsertImageFromClipboard()
        {
            if (clipboard.GetImage() is { } image)
                ImageProvider.InsertImage(ImageProvider.SelectedImageIndex + 1, image);
        }


        private DelegateCommand<CaptureImage>? _CopyToClipboardCommand;
        public DelegateCommand<CaptureImage> CopyToClipboardCommand
            => _CopyToClipboardCommand ??= new DelegateCommand<CaptureImage>(CopySelectedImageToClipboard);

        private void CopySelectedImageToClipboard(CaptureImage? obj)
        {
            if (obj is { } image)
                this.clipboard.SetImage(image.TransformedImage);
        }

        public IDropTarget DropHandler { get; }
    }
}
