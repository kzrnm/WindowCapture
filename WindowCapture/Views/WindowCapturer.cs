using Prism;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Kzrnm.WindowCapture.Views
{
    public class WindowCapturer : Control
    {
        static WindowCapturer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowCapturer), new FrameworkPropertyMetadata(typeof(WindowCapturer)));
        }

        public WindowCapturer()
        {
            ViewModelLocator.SetAutoWireViewModel(this, true);
            SetBinding(AlwaysImageAreaProperty,
                new Binding(nameof(ViewModels.WindowCapturerViewModel.AlwaysImageArea))
                {
                    Source = DataContext,
                    Mode = BindingMode.OneWayToSource,
                });
        }
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
            var window = Window.GetWindow(this);
            window.Loaded += Window_Loaded;
            window.Closing += this.Window_Closing;
        }


        private ContentControl? MainContent;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (MainContent != null) MainContent.SizeChanged -= this.Content_SizeChanged;
            this.MainContent = this.GetTemplateChild("PART_MainContent") as ContentControl;
            if (MainContent != null) MainContent.SizeChanged += this.Content_SizeChanged;
        }

        private bool isWindowLoaded = false;  
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ((Window)sender).Loaded -= this.Window_Loaded;
            isWindowLoaded = true;
            if (HasPreviewWindow) MakePreviewWindow();
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
                typeof(WindowCapturer));
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
                if (isWindowLoaded && imagePreviewWindow == null)
                    MakePreviewWindow();
            }
            else if (imagePreviewWindow is { } ipw)
            {
                ipw.Owner = null;
                ipw.Close();
            }
        }
        private void MakePreviewWindow()
        {
            var app = (PrismApplicationBase)Application.Current;
            var mainWindow = Window.GetWindow(this);
            imagePreviewWindow = app.Container.Resolve<ImagePreviewWindow>();
            imagePreviewWindow.Owner = mainWindow;
            imagePreviewWindow.Top = mainWindow.Top + mainWindow.Height / 2;
            imagePreviewWindow.Left = mainWindow.Left + mainWindow.Width / 2;
        }

        private Size expandSize;
        public event ExpandSizeChangedEventHandler? ExpandSizeChanged;
        private void Content_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var contentSize = ((ContentControl)sender).RenderSize;
            if (contentSize.Width == 0 || contentSize.Height == 0)
                return;

            var wcSize = this.RenderSize;
            var nextSize = new Size(wcSize.Width - contentSize.Width, wcSize.Height - contentSize.Height);
            UpdateExpandSize(nextSize);
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
