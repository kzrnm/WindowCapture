using GongSolutions.Wpf.DragDrop;
using Prism.Mvvm;

namespace CapApp.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public IDropTarget DropHandler { get; }
    }
}
