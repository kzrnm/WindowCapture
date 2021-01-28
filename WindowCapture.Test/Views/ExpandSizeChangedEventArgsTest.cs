using FluentAssertions;
using System.Windows;
using Xunit;

namespace Kzrnm.WindowCapture.Views
{
    public class ExpandSizeChangedEventArgsTest
    {
        [Fact]
        public void ConstructDefault()
        {
            new ExpandSizeChangedEventArgs().WidthDiff.Should().Be(0.0);
            new ExpandSizeChangedEventArgs().HeightDiff.Should().Be(0.0);
        }

        [Theory]
        [InlineData(100.0, 200.0)]
        [InlineData(10.0, 20.0)]
        [InlineData(-10.0, -20.0)]
        public void ConstructDouble(double width, double height)
        {
            var args = new ExpandSizeChangedEventArgs(width, height);
            args.WidthDiff.Should().Be(width);
            args.HeightDiff.Should().Be(height);
        }

        public static TheoryData ConstructSize_Data = new TheoryData<Size, Size, double, double>
        {
            {
                new Size(100,150),
                new Size(200,350),
                100,
                200
            },
            {
                new Size(200,350),
                new Size(100,150),
                -100,
                -200
            },
        };

        [Theory]
        [MemberData(nameof(ConstructSize_Data))]
        public void ConstructSize(Size oldSize, Size newSize, double expectedWidth, double expectedHeight)
        {
            var args = new ExpandSizeChangedEventArgs(oldSize, newSize);
            args.WidthDiff.Should().Be(expectedWidth);
            args.HeightDiff.Should().Be(expectedHeight);
        }
    }
}
