using GongSolutions.Wpf.DragDrop;
using System.Windows;

namespace Kzrnm.WindowCapture.Images
{
    public class ImageDragSource : DefaultDragHandler
    {
        public override void StartDrag(IDragInfo dragInfo)
        {
            base.StartDrag(dragInfo);
            if (dragInfo.Data is CaptureImage image)
            {
                dragInfo.DataFormat = DataFormats.GetDataFormat(DataFormats.Bitmap);
                var dataObj = new DataObject();
                dataObj.SetImage(image.TransformedImage);
                dataObj.SetImage(image.TransformedImage);
                dataObj.SetImage(image.TransformedImage);
                dataObj.SetImage(image.TransformedImage);
                dataObj.SetImage(image.TransformedImage);
                dataObj.SetImage(image.TransformedImage);
                dragInfo.DataObject = dataObj;
            }
        }
    }
}
