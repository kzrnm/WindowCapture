using FluentAssertions;
using GongSolutions.Wpf.DragDrop;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xunit;

namespace Kzrnm.WindowCapture.Images
{
    public class ImageDragSourceTests
    {
        [Fact]
        public void NotCaptureImage()
        {
            var mock = new Mock<IDragInfo>();
            mock.SetupGet(d => d.Data).Returns("string");

            new ImageDragSource().StartDrag(mock.Object);

            mock.VerifySet(d => d.DataFormat = It.IsAny<DataFormat>(), Times.Never());
            mock.VerifySet(d => d.DataObject = It.IsAny<object>(), Times.Never());
        }

        [Fact]
        public void CaptureImage()
        {
            var img = new CaptureImage(TestUtil.DummyBitmapSource(2, 2)) { JpegQualityLevel = 100 };
            img.ImageRatioSize.Width = 10;

            var mock = new Mock<IDragInfo>();
            mock.SetupGet(d => d.Data).Returns(img);
            new ImageDragSource().StartDrag(mock.Object);

            mock.VerifySet(d => d.DataFormat = DataFormats.GetDataFormat(DataFormats.Bitmap), Times.Once());
            mock.VerifySet(d => d.DataObject = It.Is(img.TransformedImage,
                new LambdaEqualityComparer<object>(VerifyDataObject)), Times.Once());
            static bool VerifyDataObject(object obj1, object obj2)
            {
                var (dataObject, img) = (obj1, obj2) switch
                {
                    (DataObject o, BitmapSource b) => (o, b),
                    (BitmapSource b, DataObject o) => (o, b),
                    _ => default,
                };

                if (dataObject == null && img == null) return false;
                return TestUtil.ImageToByte(dataObject.GetImage()).SequenceEqual(TestUtil.ImageToByte(img));
            }
        }
    }
}
