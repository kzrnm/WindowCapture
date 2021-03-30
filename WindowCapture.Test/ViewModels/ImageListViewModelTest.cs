using FluentAssertions;
using Kzrnm.WindowCapture.Images;
using Kzrnm.WindowCapture.Windows;
using Microsoft.Toolkit.Mvvm.Messaging;
using Moq;
using System.Collections.Specialized;
using System.Windows.Media.Imaging;
using Xunit;

namespace Kzrnm.WindowCapture.ViewModels
{
    public class ImageListViewModelTest
    {
        public WeakReferenceMessenger Messenger;
        public ImageProvider ImageProvider;

        public ImageListViewModelTest()
        {
            Messenger = new();
            ImageProvider = new ImageProvider(Messenger);
        }

        [Fact]
        public void RemoveSelectedImageCommand()
        {
            var clipboardMock = new Mock<IClipboardManager>();
            var viewModel = new ImageListViewModel(Messenger, clipboardMock.Object, ImageProvider);

            viewModel.RemoveSelectedImageCommand.CanExecute().Should().BeFalse();

            ImageProvider.AddImage(TestUtil.DummyBitmapSource(10, 10));
            viewModel.RemoveSelectedImageCommand.CanExecute().Should().BeTrue();
            viewModel.RemoveSelectedImageCommand.Execute();
            ImageProvider.SelectedImage.Should().BeNull();
            ImageProvider.Images.Should().BeEmpty();
        }


        [Fact]
        public void InsertImageFromClipboardCommand()
        {
            var clipboardMock = new Mock<IClipboardManager>();
            var viewModel = new ImageListViewModel(Messenger, clipboardMock.Object, ImageProvider);
            viewModel.InsertImageFromClipboardCommand.CanExecute().Should().BeFalse();

            viewModel.InsertImageFromClipboardCommand.CanExecute().Should().BeFalse();
            viewModel.InsertImageFromClipboardCommand.Execute();
            ImageProvider.Images.Should().BeEmpty();

            NotifyCollectionChangedEventArgs lastCollectionChanged = null;
            ImageProvider.Images.CollectionChanged += (_, e) => lastCollectionChanged = e;

            clipboardMock.Setup(m => m.ContainsImage()).Returns(true);
            clipboardMock.Setup(m => m.GetImage()).Returns(() => TestUtil.DummyBitmapSource(10, 10));

            viewModel.InsertImageFromClipboardCommand.CanExecute().Should().BeTrue();
            viewModel.InsertImageFromClipboardCommand.Execute();
            lastCollectionChanged.NewStartingIndex.Should().Be(0);

            viewModel.InsertImageFromClipboardCommand.CanExecute().Should().BeTrue();
            viewModel.InsertImageFromClipboardCommand.Execute();
            lastCollectionChanged.NewStartingIndex.Should().Be(1);

            ImageProvider.Images.SelectedIndex = 0;
            viewModel.InsertImageFromClipboardCommand.CanExecute().Should().BeTrue();
            viewModel.InsertImageFromClipboardCommand.Execute();
            lastCollectionChanged.NewStartingIndex.Should().Be(1);
        }

        [Fact]
        public void CopyToClipboardCommand()
        {
            ImageProvider.AddImage(TestUtil.DummyBitmapSource(10, 10));
            var img = ImageProvider.Images[0];
            img.ImageRatioSize.WidthPercentage = 200;
            var clipboardMock = new Mock<IClipboardManager>();
            var viewModel = new ImageListViewModel(Messenger, clipboardMock.Object, ImageProvider);

            BitmapSource called = null;
            clipboardMock.Setup(c => c.SetImage(It.IsAny<BitmapSource>())).Callback<BitmapSource>(img => called = img);

            viewModel.CopyToClipboardCommand.Execute(img);
            clipboardMock.Verify(c => c.SetImage(It.IsAny<BitmapSource>()), Times.Once());
            called.Should().Be(img.TransformedImage);
        }
    }
}
