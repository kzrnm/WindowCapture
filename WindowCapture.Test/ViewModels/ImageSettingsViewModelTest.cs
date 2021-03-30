using FluentAssertions;
using KzLibraries.EventHandlerHistory;
using Kzrnm.WindowCapture.Images;
using Microsoft.Toolkit.Mvvm.Messaging;
using System.Collections.Generic;
using Xunit;

namespace Kzrnm.WindowCapture.ViewModels
{
    public class ImageSettingsViewModelTest
    {
        public WeakReferenceMessenger Messenger;
        public ImageProvider ImageProvider;

        public ImageSettingsViewModelTest()
        {
            Messenger = new();
            ImageProvider = new ImageProvider(Messenger);
        }

        [Fact]
        public void Width()
        {
            ImageProvider.AddImage(TestUtil.DummyBitmapSource(4, 4));
            var viewModel = new ImageSettingsViewModel(Messenger, ImageProvider);
            using (var ph = new PropertyChangedHistory(viewModel))
            {
                ph.Should().Equal(new Dictionary<string, int> { });
                viewModel.Width = 5;
                viewModel.WidthPercentage.Should().Be(100.0 * 5 / 4);
                ph.Should().Equal(new Dictionary<string, int>
                {
                    { "Width", 1 },
                    { "WidthPercentage", 1 },
                });
            }
            using (var ph = new PropertyChangedHistory(viewModel))
            {
                ImageProvider.AddImage(TestUtil.DummyBitmapSource(6, 6));
                ph.Should().Equal(new Dictionary<string, int>
                {
                    { "Width", 1 },
                    { "Height", 1 },
                    { "SelectedImage", 1 },
                });
            }
        }

        [Fact]
        public void Height()
        {
            ImageProvider.AddImage(TestUtil.DummyBitmapSource(4, 4));
            var viewModel = new ImageSettingsViewModel(Messenger, ImageProvider);
            using (var ph = new PropertyChangedHistory(viewModel))
            {
                ph.Should().Equal(new Dictionary<string, int> { });
                viewModel.Height = 5;
                viewModel.HeightPercentage.Should().Be(100.0 * 5 / 4);
                ph.Should().Equal(new Dictionary<string, int>
                {
                    { "Height", 1 },
                    { "HeightPercentage", 1 },
                });
            }
            using (var ph = new PropertyChangedHistory(viewModel))
            {
                ImageProvider.AddImage(TestUtil.DummyBitmapSource(6, 6));
                ph.Should().Equal(new Dictionary<string, int>
                {
                    { "Width", 1 },
                    { "Height", 1 },
                    { "SelectedImage", 1 },
                });
            }
        }

        [Fact]
        public void WidthPercentage()
        {
            ImageProvider.AddImage(TestUtil.DummyBitmapSource(4, 4));
            var viewModel = new ImageSettingsViewModel(Messenger, ImageProvider);
            using (var ph = new PropertyChangedHistory(viewModel))
            {
                ph.Should().Equal(new Dictionary<string, int> { });
                viewModel.WidthPercentage = 180;
                viewModel.Width.Should().Be(7);
                viewModel.WidthPercentage.Should().Be(180.0);
                viewModel.HeightPercentage.Should().Be(180.0);
                ph.Should().Equal(new Dictionary<string, int>
                {
                    { "Width", 1 },
                    { "WidthPercentage", 1 },
                    { "Height", 1 },
                    { "HeightPercentage", 1 },
                });
            }
            using (var ph = new PropertyChangedHistory(viewModel))
            {
                ImageProvider.AddImage(TestUtil.DummyBitmapSource(6, 6));
                viewModel.WidthPercentage.Should().Be(180.0);
                viewModel.HeightPercentage.Should().Be(180.0);
                ph.Should().Equal(new Dictionary<string, int>
                {
                    { "Width", 1 },
                    { "Height", 1 },
                    { "SelectedImage", 1 },
                });
            }
        }

        [Fact]
        public void HeightPercentage()
        {
            ImageProvider.AddImage(TestUtil.DummyBitmapSource(4, 4));
            var viewModel = new ImageSettingsViewModel(Messenger, ImageProvider);
            using (var ph = new PropertyChangedHistory(viewModel))
            {
                ph.Should().Equal(new Dictionary<string, int> { });
                viewModel.HeightPercentage = 180;
                viewModel.Height.Should().Be(7);
                viewModel.WidthPercentage.Should().Be(180.0);
                viewModel.HeightPercentage.Should().Be(180.0);
                ph.Should().Equal(new Dictionary<string, int>
                {
                    { "Width", 1 },
                    { "WidthPercentage", 1 },
                    { "Height", 1 },
                    { "HeightPercentage", 1 },
                });
            }
            using (var ph = new PropertyChangedHistory(viewModel))
            {
                ImageProvider.AddImage(TestUtil.DummyBitmapSource(6, 6));
                viewModel.WidthPercentage.Should().Be(180.0);
                viewModel.HeightPercentage.Should().Be(180.0);
                ph.Should().Equal(new Dictionary<string, int>
                {
                    { "Width", 1 },
                    { "Height", 1 },
                    { "SelectedImage", 1 },
                });
            }
        }

        [Fact]
        public void ImageKind()
        {
            ImageProvider.AddImage(TestUtil.DummyBitmapSource(4, 4));
            var viewModel = new ImageSettingsViewModel(Messenger, ImageProvider);
            using (var ph = new PropertyChangedHistory(viewModel))
            {
                ph.Should().Equal(new Dictionary<string, int> { });
                viewModel.ImageKind.Should().Be(Images.ImageKind.JPG);
                viewModel.ImageKind = Images.ImageKind.PNG;
                viewModel.ImageKind.Should().Be(Images.ImageKind.PNG);
                ph.Should().Equal(new Dictionary<string, int>
                {
                    { "ImageKind", 1 },
                });
            }
            using (var ph = new PropertyChangedHistory(viewModel))
            {
                ImageProvider.AddImage(TestUtil.DummyBitmapSource(4, 4));
                viewModel.ImageKind.Should().Be(Images.ImageKind.PNG);
                ph.Should().Equal(new Dictionary<string, int>
                {
                    { "SelectedImage", 1 },
                });
            }
        }

        [Fact]
        public void IsSideCutMode()
        {
            ImageProvider.AddImage(TestUtil.DummyBitmapSource(4, 4));
            var viewModel = new ImageSettingsViewModel(Messenger, ImageProvider);
            using (var ph = new PropertyChangedHistory(viewModel))
            {
                ph.Should().Equal(new Dictionary<string, int> { });
                viewModel.IsSideCutMode.Should().BeFalse();
                viewModel.IsSideCutMode = true;
                viewModel.IsSideCutMode.Should().BeTrue();
                ph.Should().Equal(new Dictionary<string, int>
                {
                    { "IsSideCutMode", 1 },
                });
            }
            using (var ph = new PropertyChangedHistory(viewModel))
            {
                ImageProvider.AddImage(TestUtil.DummyBitmapSource(4, 4));
                viewModel.IsSideCutMode.Should().BeTrue();
                ph.Should().Equal(new Dictionary<string, int>
                {
                    { "SelectedImage", 1 },
                });
            }
        }

        [Fact]
        public void SelectedChanged()
        {
            var viewModel = new ImageSettingsViewModel(Messenger, ImageProvider);
            viewModel.HasImage.Should().BeFalse();
            viewModel.SelectedImage.Should().BeNull();
            viewModel.Width.Should().Be(0);
            viewModel.Height.Should().Be(0);
            viewModel.WidthPercentage.Should().Be(0);
            viewModel.HeightPercentage.Should().Be(0);
            viewModel.ImageKind.Should().Be(Images.ImageKind.JPG);
            viewModel.IsSideCutMode.Should().BeFalse();

            using (var ph = new PropertyChangedHistory(viewModel))
            {
                ImageProvider.AddImage(TestUtil.DummyBitmapSource(4, 4));
                viewModel.HasImage.Should().BeTrue();
                viewModel.SelectedImage.Should().NotBeNull();
                viewModel.Width.Should().Be(4);
                viewModel.Height.Should().Be(4);
                viewModel.WidthPercentage.Should().Be(100);
                viewModel.HeightPercentage.Should().Be(100);
                viewModel.ImageKind.Should().Be(Images.ImageKind.JPG);
                viewModel.IsSideCutMode.Should().BeFalse();
                ph.Should().Equal(new Dictionary<string, int>
                {
                    { "HasImage", 1},
                    { "SelectedImage", 1 },
                    { "Width", 1},
                    { "Height", 1},
                    { "WidthPercentage", 1},
                    { "HeightPercentage", 1},
                    { "ImageKind", 1 },
                    { "IsSideCutMode", 1 },
                });
            }

            ImageProvider.AddImage(TestUtil.DummyBitmapSource(5, 5));
            viewModel.HasImage.Should().BeTrue();
            viewModel.SelectedImage.Should().NotBeNull();
            viewModel.Width = 9;
            viewModel.Height = 3;
            viewModel.ImageKind = Images.ImageKind.PNG;
            viewModel.IsSideCutMode = true;
            viewModel.Width.Should().Be(9);
            viewModel.Height.Should().Be(3);
            viewModel.WidthPercentage.Should().Be(180);
            viewModel.HeightPercentage.Should().Be(60);
            viewModel.ImageKind.Should().Be(Images.ImageKind.PNG);
            viewModel.IsSideCutMode.Should().BeTrue();

            using (var ph = new PropertyChangedHistory(viewModel))
            {
                ImageProvider.SelectedImageIndex = 0;
                ph.Should().Equal(new Dictionary<string, int>
                {
                    { "SelectedImage", 1 },
                    { "Width", 1},
                    { "Height", 1},
                    { "WidthPercentage", 1},
                    { "HeightPercentage", 1},
                    { "ImageKind", 1 },
                    { "IsSideCutMode", 1 },
                });
                viewModel.HasImage.Should().BeTrue();
                viewModel.SelectedImage.Should().NotBeNull();
                viewModel.Width.Should().Be(4);
                viewModel.Height.Should().Be(4);
                viewModel.WidthPercentage.Should().Be(100);
                viewModel.HeightPercentage.Should().Be(100);
                viewModel.ImageKind.Should().Be(Images.ImageKind.JPG);
                viewModel.IsSideCutMode.Should().BeFalse();


                ImageProvider.SelectedImageIndex = 1;
                ph.Should().Equal(new Dictionary<string, int>
                {
                    { "SelectedImage", 2 },
                    { "Width", 2},
                    { "Height", 2},
                    { "WidthPercentage", 2},
                    { "HeightPercentage", 2},
                    { "ImageKind", 2 },
                    { "IsSideCutMode", 2 },
                });
                viewModel.HasImage.Should().BeTrue();
                viewModel.SelectedImage.Should().NotBeNull();
                viewModel.Width.Should().Be(9);
                viewModel.Height.Should().Be(3);
                viewModel.WidthPercentage.Should().Be(180);
                viewModel.HeightPercentage.Should().Be(60);
                viewModel.ImageKind.Should().Be(Images.ImageKind.PNG);
                viewModel.IsSideCutMode.Should().BeTrue();
            }

            using (var ph = new PropertyChangedHistory(viewModel))
            {
                ImageProvider.Images.Clear();
                ph.Should().Equal(new Dictionary<string, int>
                {
                    { "HasImage", 1},
                    { "SelectedImage", 1 },
                });
                viewModel.HasImage.Should().BeFalse();
                viewModel.SelectedImage.Should().BeNull();
                viewModel.Width.Should().Be(0);
                viewModel.Height.Should().Be(0);
                viewModel.WidthPercentage.Should().Be(0);
                viewModel.HeightPercentage.Should().Be(0);
                viewModel.ImageKind.Should().Be(Images.ImageKind.JPG);
                viewModel.IsSideCutMode.Should().BeFalse();
            }
        }
    }
}
