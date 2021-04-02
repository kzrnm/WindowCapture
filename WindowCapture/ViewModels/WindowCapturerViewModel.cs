using Kzrnm.WindowCapture.Images;
using Kzrnm.WindowCapture.Mvvm;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using System.Windows;

namespace Kzrnm.WindowCapture.ViewModels
{
    public class WindowCapturerViewModel : ObservableRecipient, IRecipient<SelectedImageChangedMessage>
    {
        public ImageProvider ImageProvider { get; }
        public WindowCapturerViewModel(ICaptureImageService captureImageService, ImageProvider imageProvider)
            : this(WeakReferenceMessenger.Default, captureImageService, imageProvider)
        { }
        public WindowCapturerViewModel(IMessenger messenger, ICaptureImageService captureImageService, ImageProvider imageProvider)
            : base(messenger)
        {
            this.DropHandler = new ImageDropTarget(captureImageService, imageProvider, true);
            this.ImageProvider = imageProvider;
            IsActive = true;
        }

        void IRecipient<SelectedImageChangedMessage>.Receive(SelectedImageChangedMessage message)
        {
            UpdateImageVisibility();
        }

        public ImageDropTarget DropHandler { get; }

        private bool _AlwaysImageArea;
        public bool AlwaysImageArea
        {
            get => _AlwaysImageArea;
            set { if (SetProperty(ref _AlwaysImageArea, value)) UpdateImageVisibility(); }
        }

        private void UpdateImageVisibility()
        {
            ImageVisibility = (_AlwaysImageArea || ImageProvider.Images.Count > 0) ? Visibility.Visible : Visibility.Collapsed;
        }
        private Visibility _ImageVisibility = Visibility.Collapsed;
        public Visibility ImageVisibility
        {
            private set => SetProperty(ref _ImageVisibility, value);
            get => _ImageVisibility;
        }

        public void OnWindowClosing()
        {
            Messenger.Send<WindowClosingMessage>();
        }
    }
}
