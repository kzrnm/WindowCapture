using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Kzrnm.WindowCapture.Views
{
    public partial class ImagePreviewWindow : Window
    {
        public ImagePreviewWindow()
        {
            InitializeComponent();
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (this.Owner != null)
                e.Cancel = true;
        }
    }
}
