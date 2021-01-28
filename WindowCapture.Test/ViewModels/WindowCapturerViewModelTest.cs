using FluentAssertions;
using KzLibraries.EventHandlerHistory;
using Kzrnm.WindowCapture.Images;
using Prism.Events;
using System.Collections.Generic;
using System.Windows;
using Xunit;

namespace Kzrnm.WindowCapture.ViewModels
{
    public class WindowCapturerViewModelTest
    {
        public EventAggregator EventAggregator;
        public ImageProvider ImageProvider;

        public WindowCapturerViewModelTest()
        {
            EventAggregator = new EventAggregator();
            ImageProvider = new ImageProvider(EventAggregator);
        }

        [Fact]
        public void ImageVisibility()
        {
            var viewModel = new WindowCapturerViewModel(EventAggregator, ImageProvider);
            using (var ph = new PropertyChangedHistory(viewModel))
            {
                ph.Should().Equal(new Dictionary<string, int> { });
                viewModel.ImageVisibility.Should().Be(Visibility.Collapsed);
                ImageProvider.AddImage(TestUtil.DummyBitmapSource(2, 2));
                viewModel.ImageVisibility.Should().Be(Visibility.Visible);
                ImageProvider.AddImage(TestUtil.DummyBitmapSource(2, 2));
                viewModel.ImageVisibility.Should().Be(Visibility.Visible);
                ph.Should().Equal(new Dictionary<string, int>
                {
                    { "ImageVisibility", 1 }
                });
                ImageProvider.Images.Clear();
                viewModel.ImageVisibility.Should().Be(Visibility.Collapsed);
                ph.Should().Equal(new Dictionary<string, int>
                {
                    { "ImageVisibility", 2 }
                });
            }
            using (var ph = new PropertyChangedHistory(viewModel))
            {
                ph.Should().Equal(new Dictionary<string, int> { });
                viewModel.AlwaysImageArea = true;
                viewModel.ImageVisibility.Should().Be(Visibility.Visible);
                ph.Should().Equal(new Dictionary<string, int>
                {
                    { "AlwaysImageArea", 1 },
                    { "ImageVisibility", 1 }
                });
                ImageProvider.AddImage(TestUtil.DummyBitmapSource(2, 2));
                viewModel.ImageVisibility.Should().Be(Visibility.Visible);
                ph.Should().Equal(new Dictionary<string, int>
                {
                    { "AlwaysImageArea", 1 },
                    { "ImageVisibility", 1 }
                });
            }
        }
    }
}
