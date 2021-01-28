using FluentAssertions;
using KzLibraries.EventHandlerHistory;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xunit;

namespace Kzrnm.WindowCapture.Images
{
    public class ImageProviderTests
    {
        public EventAggregator EventAggregator;
        public ImageProvider ImageProvider;

        public ImageProviderTests()
        {
            EventAggregator = new EventAggregator();
            ImageProvider = new ImageProvider(EventAggregator);
        }

        [Fact]
        public void AddImageTest()
        {
            var imagesCollectionChangedHistory = new CollectionChangedHistory(this.ImageProvider.Images);
            int selectedImageChangedCount = 0;
            Action<(CaptureImage, CaptureImage)> increment = _ => selectedImageChangedCount++;
            EventAggregator.GetEvent<SelectedImageChangedEvent>().Subscribe(increment);

            this.ImageProvider.SelectedImage.Should().BeNull();
            selectedImageChangedCount.Should().Be(0);

            this.ImageProvider.AddImage(BitmapSource.Create(
                    2, 2, 96, 96,
                    PixelFormats.Indexed1,
                    new BitmapPalette(new[] { Colors.Transparent }),
                    new byte[] { 0, 0, 0, 0 }, 1));

            this.ImageProvider.SelectedImageIndex.Should().Be(0);
            this.ImageProvider.Images.Should().ContainSingle();

            selectedImageChangedCount.Should().Be(1);

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
            selectedImageChangedCount.Should().Be(2);
        }

        [Fact]
        public void SelectedImageChangedTest()
        {
            var eventArgsList = new List<(CaptureImage OldImage, CaptureImage NewImage)>();
            Action<(CaptureImage, CaptureImage)> addEventArgs = t => eventArgsList.Add(t);
            EventAggregator.GetEvent<SelectedImageChangedEvent>().Subscribe(addEventArgs);
            var images = new[]{
                new CaptureImage(TestUtil.DummyBitmapSource(2, 2)),
                new CaptureImage(TestUtil.DummyBitmapSource(2, 2)),
            };

            this.ImageProvider.Images.Add(images[0]);
            eventArgsList.Should().HaveCount(1);
            eventArgsList[0].OldImage.Should().BeNull();
            eventArgsList[0].NewImage.Should().Be(images[0]);

            this.ImageProvider.Images.Add(images[1]);
            eventArgsList.Should().HaveCount(2);
            eventArgsList[1].OldImage.Should().Be(images[0]);
            eventArgsList[1].NewImage.Should().Be(images[1]);

            this.ImageProvider.Images.Clear();
            eventArgsList.Should().HaveCount(3);
            eventArgsList[2].OldImage.Should().Be(images[1]);
            eventArgsList[2].NewImage.Should().BeNull();
        }
    }
}
