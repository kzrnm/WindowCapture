using GongSolutions.Wpf.DragDrop;
using Kzrnm.WindowCapture.Images;
using Kzrnm.WindowCapture.Windows;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace Kzrnm.WindowCapture.ViewModels
{
    public class ImageListViewModel : BindableBase
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IClipboardManager clipboard;
        public ImageProvider ImageProvider { get; }
        public ImageListViewModel(IEventAggregator ea, IClipboardManager clipboardManager, ImageProvider imageProvider)
        {
            this.eventAggregator = ea;
            this.ImageProvider = imageProvider;
            this.clipboard = clipboardManager;
            this.DropHandler = new ImageDropTarget(imageProvider);
            eventAggregator.GetEvent<SelectedImageChangedEvent>().Subscribe(SelectedImageChangedAction);
        }

        private void SelectedImageChangedAction((CaptureImage? oldImage, CaptureImage? newImage) t)
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
