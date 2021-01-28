using FluentAssertions;
using Xunit;
using System.Collections.Generic;
using KzLibraries.EventHandlerHistory;

namespace Kzrnm.WindowCapture.Images
{
    public class ImageRatioSizeTests
    {
        [Theory]
        [InlineData(1133, 11)]
        [InlineData(311, 11)]
        [InlineData(112, 1)]
        [InlineData(11, 111)]
        [InlineData(1, 1111)]
        public void IsNotChangedTest(int width, int height)
        {
            new ImageRatioSize(width, height).IsNotChanged.Should().BeTrue();
            new ImageRatioSize(width, height, 100).IsNotChanged.Should().BeTrue();
            new ImageRatioSize(width, height, 100, 100).IsNotChanged.Should().BeTrue();
        }

        [Theory]
        [InlineData(1133, 11, 100, 50)]
        [InlineData(311, 11, 50, 100)]
        [InlineData(112, 1, 50, 50)]
        [InlineData(11, 111, 100.5, 50)]
        [InlineData(1, 1111, 0, 50)]
        public void IsChangedTest(int width, int height, double widthPercentage, double heightPercentage)
        {
            new ImageRatioSize(width, height, widthPercentage, heightPercentage).IsNotChanged
                .Should().BeFalse();
        }

        [Theory]
        [InlineData(1133, 11, 100, 50)]
        [InlineData(311, 11, 50, 100)]
        [InlineData(112, 1, 50, 50)]
        [InlineData(11, 111, 100.5, 0)]
        [InlineData(1, 1111, 0, 50)]
        public void SizeTest(int width, int height, double widthPercentage, double heightPercentage)
        {
            var ratioSize = new ImageRatioSize(width, height, widthPercentage, heightPercentage);
            ratioSize.OrigWidth.Should().Be(width);
            ratioSize.OrigHeight.Should().Be(height);

            ratioSize.WidthPercentage.Should().Be(widthPercentage);
            ratioSize.HeightPercentage.Should().Be(heightPercentage);

            ratioSize.Width.Should().Be((int)(width * widthPercentage / 100));
            ratioSize.Height.Should().Be((int)(height * heightPercentage / 100));
        }

        [Fact]
        public void SizeChangeTest()
        {
            var ratioSize = new ImageRatioSize(10, 10, 50, 100);
            ratioSize.Width.Should().Be(5);
            ratioSize.Height.Should().Be(10);

            using (var popertyChangedHistory = new PropertyChangedHistory(ratioSize))
            {
                ratioSize.WidthPercentage = 80;
                ratioSize.WidthPercentage.Should().Be(80);
                ratioSize.HeightPercentage.Should().Be(100);
                ratioSize.Width.Should().Be(8);
                ratioSize.Height.Should().Be(10);
                popertyChangedHistory.Should().BeEquivalentTo(new Dictionary<string, int>
                {
                    {nameof(ratioSize.Width) ,1},
                    {nameof(ratioSize.WidthPercentage),1},
                });
            }

            using (var popertyChangedHistory = new PropertyChangedHistory(ratioSize))
            {
                ratioSize.HeightPercentage = 10;
                ratioSize.WidthPercentage.Should().Be(80);
                ratioSize.HeightPercentage.Should().Be(10);
                ratioSize.Width.Should().Be(8);
                ratioSize.Height.Should().Be(1);
                popertyChangedHistory.Should().BeEquivalentTo(new Dictionary<string, int>
                {
                    {nameof(ratioSize.Height) ,1},
                    {nameof(ratioSize.HeightPercentage),1},
                });
            }

            using (var popertyChangedHistory = new PropertyChangedHistory(ratioSize))
            {
                ratioSize.Width = 100;
                ratioSize.WidthPercentage.Should().Be(1000);
                ratioSize.HeightPercentage.Should().Be(10);
                ratioSize.Width.Should().Be(100);
                ratioSize.Height.Should().Be(1);
                popertyChangedHistory.Should().BeEquivalentTo(new Dictionary<string, int>
                {
                    {nameof(ratioSize.Width) ,1},
                    {nameof(ratioSize.WidthPercentage),1},
                });
            }

            using (var popertyChangedHistory = new PropertyChangedHistory(ratioSize))
            {
                ratioSize.Height = 15;
                ratioSize.WidthPercentage.Should().Be(1000);
                ratioSize.HeightPercentage.Should().Be(150);
                ratioSize.Width.Should().Be(100);
                ratioSize.Height.Should().Be(15);
                popertyChangedHistory.Should().BeEquivalentTo(new Dictionary<string, int>
                {
                    {nameof(ratioSize.Height) ,1},
                    {nameof(ratioSize.HeightPercentage),1},
                });
            }
        }
    }
}
