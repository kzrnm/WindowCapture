using System;
using System.Windows;

namespace Kzrnm.WindowCapture.Views
{
    public delegate void ExpandSizeChangedEventHandler(object? sender, ExpandSizeChangedEventArgs e);
    public class ExpandSizeChangedEventArgs : EventArgs
    {
        public double WidthDiff { get; }
        public double HeightDiff { get; }
        public ExpandSizeChangedEventArgs(double widthDiff = 0, double heightDiff = 0)
        {
            this.WidthDiff = widthDiff;
            this.HeightDiff = heightDiff;
        }

        public ExpandSizeChangedEventArgs(Size oldSize, Size newSize)
        {
            this.WidthDiff = newSize.Width - oldSize.Width;
            this.HeightDiff = newSize.Height - oldSize.Height;
        }
    }
}
