using Kzrnm.WindowCapture.ViewModels;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Kzrnm.WindowCapture.Views
{
    public class WindowCapturer : DockPanel
    {
        static WindowCapturer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowCapturer), new FrameworkPropertyMetadata(typeof(WindowCapturer)));
        }

        public WindowCapturer()
        {
            Children.Add(ImageSettings);
            Children.Add(ImageListView);
            SetDock(ImageSettings, Dock.Right);
            SetDock(ImageListView, Dock.Bottom);

            this.ViewModel = Ioc.Default.GetService<WindowCapturerViewModel>();
            SetBinding(AlwaysImageAreaProperty,
                new Binding(nameof(WindowCapturerViewModel.AlwaysImageArea))
                {
                    Source = ViewModel,
                    Mode = BindingMode.OneWayToSource,
                });
            ImageSettings.SetBinding(WidthProperty,
                new Binding(nameof(SettingsWidth))
                {
                    Source = this,
                    Mode = BindingMode.OneWay,
                });
            ImageListView.SetBinding(HeightProperty,
                new Binding(nameof(ListHeight))
                {
                    Source = this,
                    Mode = BindingMode.OneWay,
                });

            var visibilityBinding = new Binding(nameof(ViewModel.ImageVisibility))
            {
                Source = ViewModel,
                Mode = BindingMode.OneWay,
            };
            ImageSettings.SetBinding(VisibilityProperty, visibilityBinding);
            ImageListView.SetBinding(VisibilityProperty, visibilityBinding);
            Loaded += this.OnLoaded;
        }

        public WindowCapturerViewModel? ViewModel { get; }
        public ImageSettings ImageSettings { get; } = new();
        public ImageListView ImageListView { get; } = new();

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            if (HasPreviewWindow) MakePreviewWindow(window);
            window.Closing += this.Window_Closing;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!AlwaysImageArea)
                UpdateExpandSize(new Size(0, 0));
        }

        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.Register(
                nameof(ImageWidth),
                typeof(double),
                typeof(WindowCapturer),
                new PropertyMetadata(double.NaN));
        public double ImageWidth
        {
            get => (double)GetValue(ImageWidthProperty);
            set => SetValue(ImageWidthProperty, value);
        }
        public static readonly DependencyProperty ListHeightProperty =
            DependencyProperty.Register(
                nameof(ListHeight),
                typeof(double),
                typeof(WindowCapturer));
        public double ListHeight
        {
            get => (double)GetValue(ListHeightProperty);
            set => SetValue(ListHeightProperty, value);
        }

        public static readonly DependencyProperty SettingsWidthProperty =
            DependencyProperty.Register(
                nameof(SettingsWidth),
                typeof(double),
                typeof(WindowCapturer),
                new PropertyMetadata(double.NaN));
        public double SettingsWidth
        {
            get => (double)GetValue(SettingsWidthProperty);
            set => SetValue(SettingsWidthProperty, value);
        }

        public static readonly DependencyProperty AlwaysImageAreaProperty =
            DependencyProperty.Register(
                nameof(AlwaysImageArea),
                typeof(bool),
                typeof(WindowCapturer),
                new PropertyMetadata(false));
        public bool AlwaysImageArea
        {
            get => (bool)GetValue(AlwaysImageAreaProperty);
            set => SetValue(AlwaysImageAreaProperty, value);
        }

        private ImagePreviewWindow? imagePreviewWindow;
        public static readonly DependencyProperty HasPreviewWindowProperty =
            DependencyProperty.Register(
                nameof(HasPreviewWindow),
                typeof(bool),
                typeof(WindowCapturer),
                new PropertyMetadata(true, (d, e) => ((WindowCapturer)d).OnHasPreviewWindowChanged((bool)e.NewValue)));
        public bool HasPreviewWindow
        {
            get => (bool)GetValue(HasPreviewWindowProperty);
            set => SetValue(HasPreviewWindowProperty, value);
        }
        private void OnHasPreviewWindowChanged(bool newValue)
        {
            if (newValue)
            {
                if (imagePreviewWindow == null)
                    MakePreviewWindow(Window.GetWindow(this));
            }
            else if (imagePreviewWindow is { } ipw)
            {
                ipw.Owner = null;
                ipw.Close();
            }
        }
        private void MakePreviewWindow(Window window)
        {
            imagePreviewWindow = new();
            imagePreviewWindow.Owner = window;
            imagePreviewWindow.Top = window.Top + window.Height / 2;
            imagePreviewWindow.Left = window.Left + window.Width / 2;
        }

        private Size expandSize;
        public event ExpandSizeChangedEventHandler? ExpandSizeChanged;
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            //var contentSize = ((ContentControl)sender).RenderSize;
            //if (contentSize.Width == 0 || contentSize.Height == 0)
            //    return;

            //var wcSize = this.RenderSize;
            //var nextSize = new Size(wcSize.Width - contentSize.Width, wcSize.Height - contentSize.Height);
            //UpdateExpandSize(nextSize);
        }
        private void UpdateExpandSize(Size nextSize)
        {
            if (this.expandSize == nextSize)
                return;

            this.ExpandSizeChanged?.Invoke(this, new ExpandSizeChangedEventArgs(this.expandSize, nextSize));
            this.expandSize = nextSize;
        }
    }
}
