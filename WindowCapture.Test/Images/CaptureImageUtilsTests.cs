using FluentAssertions;
using Xunit;

namespace Kzrnm.WindowCapture.Images
{
    public class CaptureImageUtilsTests
    {

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
            CaptureImageUtils.IsImageFile(name).Should().Be(expected);
        }
    }
}
