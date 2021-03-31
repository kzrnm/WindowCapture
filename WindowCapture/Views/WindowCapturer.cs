using Kzrnm.WindowCapture.Images;
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
using DragDrop = GongSolutions.Wpf.DragDrop.DragDrop;

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

            this.ViewModel = Ioc.Default.GetService<WindowCapturerViewModel>()!;
            if (this.ViewModel is null)
                throw new NullReferenceException($"{nameof(WindowCapturerViewModel)} is not Registered");
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

            Loaded += this.OnLoaded;
            Unloaded += this.OnUnloaded;
        }

        public WindowCapturerViewModel ViewModel { get; }
        public ImageSettings ImageSettings { get; } = new();
        public ImageListView ImageListView { get; } = new();

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
        }

        private void ViewModel_PropertyChanging(object? sender, PropertyChangingEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.ImageVisibility):
                    if (ViewModel.ImageVisibility == Visibility.Visible)
                        OnImageVisibilityChanged(false);
                    break;
            }
        }

        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.ImageVisibility):
                    if (ViewModel.ImageVisibility == Visibility.Visible)
                        OnImageVisibilityChanged(true);
                    break;
            }
        }

        private void OnImageVisibilityChanged(bool newIsVisible)
        {
            if (AlwaysImageArea) return;
            if (Window.GetWindow(this) is not { } window) return;

            var width = ImageSettings.ActualWidth;
            var height = ImageListView.ActualHeight;
            if (!newIsVisible)
            {
                width = -width;
                height = -height;
            }
            window.Width += width;
            window.Height += height;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is not { } window) return;
            if (HasPreviewWindow) MakePreviewWindow(window);
            window.Closing += (_, _) => ViewModel.OnWindowClosing();

            var visibilityBinding = new Binding(nameof(ViewModel.ImageVisibility))
            {
                Source = ViewModel,
                Mode = BindingMode.OneWay,
            };
            ImageSettings.SetBinding(VisibilityProperty, visibilityBinding);
            ImageListView.SetBinding(VisibilityProperty, visibilityBinding);

            ViewModel.PropertyChanging += this.ViewModel_PropertyChanging;
            ViewModel.PropertyChanged += this.ViewModel_PropertyChanged;

            DragDrop.SetIsDropTarget(this, true);
            DragDrop.SetDropEventType(this, GongSolutions.Wpf.DragDrop.EventType.Bubbled);
            DragDrop.SetDropHandler(this, ViewModel.DropHandler);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.PropertyChanging -= this.ViewModel_PropertyChanging;
            ViewModel.PropertyChanged -= this.ViewModel_PropertyChanged;
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
                new PropertyMetadata(true));
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
    }
}
