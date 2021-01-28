using Moq;
using FluentAssertions;
using KzLibraries.EventHandlerHistory;
using Kzrnm.WindowCapture.Images;
using Kzrnm.WindowCapture.Windows;
using Prism.Events;
using System.Windows;
using System.Collections.Generic;
using Xunit;
using System.Windows.Media.Imaging;

namespace Kzrnm.WindowCapture.ViewModels
{
    public class ImagePreviewWindowViewModelTest
    {
        public EventAggregator EventAggregator;
        public ImageProvider ImageProvider;

        public ImagePreviewWindowViewModelTest()
        {
            EventAggregator = new EventAggregator();
            ImageProvider = new ImageProvider(EventAggregator);
        }

        [Fact]
        public void CopyToClipboardCommand()
        {
            ImageProvider.AddImage(TestUtil.DummyBitmapSource(10, 10));
            var img = ImageProvider.Images[0];
            var clipboardMock = new Mock<IClipboardManager>();
            var viewModel = new ImagePreviewWindowViewModel(EventAggregator, clipboardMock.Object, ImageProvider);

            BitmapSource called = null;
            clipboardMock.Setup(c => c.SetImage(It.IsAny<BitmapSource>())).Callback<BitmapSource>(img => called = img);

            viewModel.CopyToClipboardCommand.Execute(img);
            clipboardMock.Verify(c => c.SetImage(It.IsAny<BitmapSource>()), Times.Once());
            called.Should().Be(img.TransformedImage);
        }

        [Fact]
        public void PasteImageFromClipboardCommand()
        {
            var img = TestUtil.DummyBitmapSource(10, 10);
            var clipboardMock = new Mock<IClipboardManager>();
            var viewModel = new ImagePreviewWindowViewModel(EventAggregator, clipboardMock.Object, ImageProvider);

            viewModel.PasteImageFromClipboardCommand.CanExecute().Should().BeFalse();
            viewModel.PasteImageFromClipboardCommand.Execute();
            ImageProvider.Images.Should().BeEmpty();

            clipboardMock.Setup(m => m.ContainsImage()).Returns(true);
            clipboardMock.Setup(m => m.GetImage()).Returns(img);

            viewModel.UpdateCanClipboardCommand();
            viewModel.PasteImageFromClipboardCommand.CanExecute().Should().BeTrue();
            viewModel.PasteImageFromClipboardCommand.Execute();
            ImageProvider.Images.Should().ContainSingle();
        }

        [Fact]
        public void SelectedImage()
        {
            var clipboardMock = new Mock<IClipboardManager>();
            var viewModel = new ImagePreviewWindowViewModel(EventAggregator, clipboardMock.Object, ImageProvider);

            using (var ph = new PropertyChangedHistory(viewModel))
            {
                viewModel.SelectedImage.Should().BeNull();
                ph.Should().Equal(new Dictionary<string, int> { });
                ImageProvider.AddImage(TestUtil.DummyBitmapSource(4, 4));
                viewModel.SelectedImage.Should().NotBeNull();
                ph.Should().Equal(new Dictionary<string, int>
                {
                    { "Visibility",1 },
                    { "SelectedImage",1 },
                });
                ImageProvider.Images.Clear();
                viewModel.SelectedImage.Should().BeNull();
                ph.Should().Equal(new Dictionary<string, int>
                {
                    { "Visibility",2 },
                    { "SelectedImage",2 },
                });
            }
        }

        [Fact]
        public void Visibility()
        {
            var clipboardMock = new Mock<IClipboardManager>();
            var viewModel = new ImagePreviewWindowViewModel(EventAggregator, clipboardMock.Object, ImageProvider);

            using (var ph = new PropertyChangedHistory(viewModel))
            {
                viewModel.Visibility.Should().Be(System.Windows.Visibility.Collapsed);
                ph.Should().Equal(new Dictionary<string, int> { });
                ImageProvider.AddImage(TestUtil.DummyBitmapSource(4, 4));
                viewModel.Visibility.Should().Be(System.Windows.Visibility.Visible);
                ph.Should().Equal(new Dictionary<string, int>
                {
                    { "Visibility",1 },
                    { "SelectedImage",1 },
                });
                ImageProvider.Images.Clear();
                viewModel.Visibility.Should().Be(System.Windows.Visibility.Collapsed);
                ph.Should().Equal(new Dictionary<string, int>
                {
                    { "Visibility",2 },
                    { "SelectedImage",2 },
                });
            }
        }

        [Fact]
        public void ClearImageCommand()
        {
            var clipboardMock = new Mock<IClipboardManager>();
            var viewModel = new ImagePreviewWindowViewModel(EventAggregator, clipboardMock.Object, ImageProvider);
            ImageProvider.AddImage(TestUtil.DummyBitmapSource(4, 4));
            viewModel.ClearImageCommand.Execute();
            ImageProvider.Images.Should().BeEmpty();
        }
    }
}