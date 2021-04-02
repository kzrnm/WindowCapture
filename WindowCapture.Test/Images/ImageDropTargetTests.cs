using GongSolutions.Wpf.DragDrop;
using Kzrnm.WindowCapture.Images;
using Microsoft.Toolkit.Mvvm.Messaging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Xunit;

namespace Kzrnm.WindowCapture.Images
{
    public class ImageDropTargetTests
    {
        private readonly Mock<ICaptureImageService> captureImageMock = new();
        private readonly ImageProvider imageProvider;
        private readonly ImageDropTarget dropTarget;
        public ImageDropTargetTests()
        {
            var captureImageService = captureImageMock.Object;
            imageProvider = new(WeakReferenceMessenger.Default, captureImageService);
            dropTarget = new(captureImageService, imageProvider);
        }

        [UIFact]
        public void SameVisualSource()
        {
            var mock = new Mock<IDropInfo>();

            var elm = new ListView();
            mock.SetupGet(d => d.VisualTarget).Returns(elm);

            var dragInfoMock = new Mock<IDragInfo>();
            dragInfoMock.SetupGet(d => d.VisualSource).Returns(elm);
            mock.SetupGet(d => d.DragInfo).Returns(dragInfoMock.Object);

            dropTarget.DragOver(mock.Object);
            dropTarget.Drop(mock.Object);
            mock.VerifySet(d => d.NotHandled = It.IsAny<bool>(), Times.Never());
        }
    }
}
