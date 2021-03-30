using FluentAssertions;
using KzLibraries.EventHandlerHistory;
using Kzrnm.WindowCapture.Images;
using Kzrnm.WindowCapture.Windows;
using Microsoft.Toolkit.Mvvm.Messaging;
using Moq;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Xunit;

namespace Kzrnm.WindowCapture.ViewModels
{
    public class ImagePreviewWindowViewModelTest
    {
        public WeakReferenceMessenger Messenger;
        public ImageProvider ImageProvider;

        public ImagePreviewWindowViewModelTest()
        {
            Messenger = new ();
            ImageProvider = new ImageProvider(Messenger);
        }

        [Fact]
        public void CopyToClipboardCommand()
        {
            ImageProvider.AddImage(TestUtil.DummyBitmapSource(10, 10));
            var img = ImageProvider.Images[0];
            img.ImageRatioSize.WidthPercentage = 200;
            var clipboardMock = new Mock<IClipboardManager>();
            var viewModel = new ImagePreviewWindowViewModel(Messenger, clipboardMock.Object, ImageProvider);

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
            var viewModel = new ImagePreviewWindowViewModel(Messenger, clipboardMock.Object, ImageProvider);

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
            var viewModel = new ImagePreviewWindowViewModel(Messenger, clipboardMock.Object, ImageProvider);

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
            var viewModel = new ImagePreviewWindowViewModel(Messenger, clipboardMock.Object, ImageProvider);

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
            var viewModel = new ImagePreviewWindowViewModel(Messenger, clipboardMock.Object, ImageProvider);
            ImageProvider.AddImage(TestUtil.DummyBitmapSource(4, 4));
            viewModel.ClearImageCommand.Execute();
            ImageProvider.Images.Should().BeEmpty();
        }
    }
}