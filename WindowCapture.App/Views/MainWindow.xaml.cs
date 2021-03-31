using Kzrnm.WindowCapture.Views;
using System.Windows;

namespace CapApp.Views
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void WindowCapturer_ExpandSizeChanged(object sender, ExpandSizeChangedEventArgs e)
        {
            if (e.WidthDiff < 0)
            {
                this.MinWidth += e.WidthDiff;
                this.Width += e.WidthDiff;
            }
            else if (e.WidthDiff > 0)
            {
                this.Width += e.WidthDiff;
                this.MinWidth += e.WidthDiff;
            }

            if (e.HeightDiff < 0)
            {
                this.MinHeight += e.HeightDiff;
                this.Height += e.HeightDiff;
            }
            else if (e.HeightDiff > 0)
            {
                this.Height += e.HeightDiff;
                this.MinHeight += e.HeightDiff;
            }
        }

        private void StackPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
    }
}
