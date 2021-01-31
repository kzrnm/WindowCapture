using GongSolutions.Wpf.DragDrop;
using Kzrnm.WindowCapture.Images;
using Prism.Mvvm;

namespace CapApp.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel(ImageProvider imageProvider)
        {
            this.DropHandler = new ImageDropTarget(imageProvider, true);
        }
        public IDropTarget DropHandler { get; }
    }
}
