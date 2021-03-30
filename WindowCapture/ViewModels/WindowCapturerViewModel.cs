using Kzrnm.WindowCapture.Images;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Prism.Events;
using System;
using System.Windows;

namespace Kzrnm.WindowCapture.ViewModels
{
    public class WindowCapturerViewModel : ObservableObject
    {
        private readonly IEventAggregator eventAggregator;
        public ImageProvider ImageProvider { get; }
        public WindowCapturerViewModel(IEventAggregator ea, ImageProvider imageProvider)
        {
            this.ImageProvider = imageProvider;
            this.eventAggregator = ea;

            SelectedImageChanged = _ => UpdateImageVisibility();
            eventAggregator.GetEvent<SelectedImageChangedEvent>().Subscribe(SelectedImageChanged);
        }

        private readonly Action<(CaptureImage?, CaptureImage?)> SelectedImageChanged;

        private bool _AlwaysImageArea;
        public bool AlwaysImageArea
        {
            get => _AlwaysImageArea;
            set { if (SetProperty(ref _AlwaysImageArea, value)) UpdateImageVisibility(); }
        }

        private Visibility _ImageVisibility = Visibility.Collapsed;
        private bool UpdateImageVisibility()
            => SetProperty(ref _ImageVisibility,
                _AlwaysImageArea || ImageProvider.Images.Count > 0 ? Visibility.Visible : Visibility.Collapsed,
                nameof(ImageVisibility));
        public Visibility ImageVisibility => _ImageVisibility;
    }
}
