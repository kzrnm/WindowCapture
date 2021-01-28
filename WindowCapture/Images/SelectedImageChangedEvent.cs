using Prism.Events;
using System;

namespace Kzrnm.WindowCapture.Images
{
    public class SelectedImageChangedEvent : PubSubEvent<(CaptureImage? oldImage, CaptureImage? newImage)> { }
}
