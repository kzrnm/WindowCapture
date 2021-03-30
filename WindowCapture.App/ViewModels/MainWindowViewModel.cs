using GongSolutions.Wpf.DragDrop;
using Kzrnm.WindowCapture.Images;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace CapApp.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel(ImageProvider imageProvider)
        {
            this.DropHandler = new ImageDropTarget(imageProvider, true);
        }
        public IDropTarget DropHandler { get; }
    }
}
