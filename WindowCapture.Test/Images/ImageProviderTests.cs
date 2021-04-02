using FluentAssertions;
using KzLibraries.EventHandlerHistory;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xunit;

namespace Kzrnm.WindowCapture.Images
{
    public class ImageProviderTests : IDisposable, IRecipient<SelectedImageChangedMessage>
    {
        public WeakReferenceMessenger Messenger;
        public ImageProvider ImageProvider;
        public List<SelectedImageChangedMessage> selectedImageChangedHistories = new();
        public ImageProviderTests()
        {
            Messenger = new();
            ImageProvider = new ImageProvider(Messenger, new CaptureImageService());
            Messenger.Register(this);
        }

        void IDisposable.Dispose() => Messenger.UnregisterAll(this);
        void IRecipient<SelectedImageChangedMessage>.Receive(SelectedImageChangedMessage message)
            => selectedImageChangedHistories.Add(message);

        [Fact]
        public void AddImageTest()
        {
            var imagesCollectionChangedHistory = new CollectionChangedHistory(this.ImageProvider.Images);
            this.ImageProvider.SelectedImage.Should().BeNull();
            selectedImageChangedHistories.Should().HaveCount(0);

            this.ImageProvider.AddImage(BitmapSource.Create(
                    2, 2, 96, 96,
                    PixelFormats.Indexed1,
                    new BitmapPalette(new[] { Colors.Transparent }),
                    new byte[] { 0, 0, 0, 0 }, 1));

            this.ImageProvider.SelectedImageIndex.Should().Be(0);
            this.ImageProvider.Images.Should().ContainSingle();

            selectedImageChangedHistories.Should().HaveCount(1);

            this.ImageProvider.AddImage(
                BitmapSource.Create(
                    4, 4, 96, 96,
                    PixelFormats.Indexed1,
                    new BitmapPalette(new[] { Colors.Transparent }),
                    new byte[] { 0, 0, 0, 0 }, 1)
                );

            this.ImageProvider.SelectedImageIndex.Should().Be(1);
            this.ImageProvider.Images.Should().HaveCount(2);
            imagesCollectionChangedHistory.Should().HaveCount(2);
            imagesCollectionChangedHistory[0].Action
                .Should()
                .Be(NotifyCollectionChangedAction.Add);

            this.ImageProvider.SelectedImage.Should().NotBeNull();
            this.ImageProvider.SelectedImage!.ImageSource.Height.Should().Be(4);
            selectedImageChangedHistories.Should().HaveCount(2);
        }

        [Fact]
        public void SelectedImageChangedTest()
        {
            var images = new[]{
                new CaptureImage(TestUtil.DummyBitmapSource(2, 2)),
                new CaptureImage(TestUtil.DummyBitmapSource(2, 2)),
            };

            this.ImageProvider.Images.Add(images[0]);
            selectedImageChangedHistories.Should().HaveCount(1);
            selectedImageChangedHistories[0].OldValue.Should().BeNull();
            selectedImageChangedHistories[0].NewValue.Should().Be(images[0]);

            this.ImageProvider.Images.Add(images[1]);
            selectedImageChangedHistories.Should().HaveCount(2);
            selectedImageChangedHistories[1].OldValue.Should().Be(images[0]);
            selectedImageChangedHistories[1].NewValue.Should().Be(images[1]);

            this.ImageProvider.Images.Clear();
            selectedImageChangedHistories.Should().HaveCount(3);
            selectedImageChangedHistories[2].OldValue.Should().Be(images[1]);
            selectedImageChangedHistories[2].NewValue.Should().BeNull();
        }
    }
}
