using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Kzrnm.WindowCapture.Images
{
    public class CaptureImageTests
    {
        [Fact]
        public void ConstructorTest()
        {
            new CaptureImage(TestUtil.DummyBitmapSource(2, 2)).JpegQualityLevel.Should().Be(100);
            CaptureImage.DefaultJpegQualityLevel = 85;
            new CaptureImage(TestUtil.DummyBitmapSource(2, 2)).JpegQualityLevel.Should().Be(85);
        }

        public static TheoryData IsMatchTest_Data = new TheoryData<bool, string>
        {
           {true, "hoge.jpeg" },
           {true, "hoge.jpg" },
           {true, "hoge.png" },
           {true, "hoge.bmp" },
           {false, "hoge.jpegx" },
           {false, "hoge.jpgx" },
           {false, "hoge.pngx" },
           {false, "hoge.bmpx" },
           {false, "hoge.exe" },
        };

        [Theory]
        [MemberData(nameof(IsMatchTest_Data))]
        public void IsMatchTest(bool expected, string name)
        {
            CaptureImage.IsImageFile(name).Should().Be(expected);
        }

        public static TheoryData ToByteArrayTest_Data = new TheoryData<CaptureImage, byte[], int>()
        {
            {
                new CaptureImage(TestUtil.DummyBitmapSource(2, 2)) { JpegQualityLevel = 100 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                631
            },
            {
                new CaptureImage(TestUtil.DummyBitmapSource(2, 2)) { JpegQualityLevel = 10 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                630
            },
            {
                new CaptureImage(TestUtil.DummyBitmapSource(2, 2)) { ImageKind = ImageKind.PNG, JpegQualityLevel = 10 },
                new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A },
                147
            },
        };

        [Theory]
        [MemberData(nameof(ToByteArrayTest_Data))]
        public void ToByteArrayTest(CaptureImage ci, IEnumerable<byte> startsWith, int length)
        {
            ci.ToByteArray()
                .Should()
                .StartWith(startsWith)
                .And
                .HaveCount(length);
        }
    }
}
