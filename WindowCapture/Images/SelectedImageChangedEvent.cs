using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace Kzrnm.WindowCapture.Images
{
    public class SelectedImageChangedMessage : PropertyChangedMessage<CaptureImage?>
    {
        public SelectedImageChangedMessage(object sender, string? propertyName, CaptureImage? oldValue, CaptureImage? newValue) : base(sender, propertyName, oldValue, newValue)
        {
        }
    }
}
