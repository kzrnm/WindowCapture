using FluentAssertions;
using Xunit;

namespace Kzrnm.WindowCapture.Images
{
    public class CaptureImageServiceTests
    {
        public static TheoryData IsImageFile_Data = new TheoryData<bool, string>
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
        [MemberData(nameof(IsImageFile_Data))]
        public void IsImageFile(bool expected, string name)
        {
            new CaptureImageService().IsImageFile(name).Should().Be(expected);
        }

        public static TheoryData IsJpegFile_Data = new TheoryData<bool, string>
        {
           {true, "hoge.jpeg" },
           {true, "hoge.jpg" },
           {false, "hoge.png" },
           {false, "hoge.bmp" },
           {false, "hoge.jpegx" },
           {false, "hoge.jpgx" },
           {false, "hoge.pngx" },
           {false, "hoge.bmpx" },
           {false, "hoge.exe" },
        };

        [Theory]
        [MemberData(nameof(IsJpegFile_Data))]
        public void IsJpegFile(bool expected, string name)
        {
            new CaptureImageService().IsJpegFile(name).Should().Be(expected);
        }
    }
}
