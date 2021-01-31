using GongSolutions.Wpf.DragDrop;
using Kzrnm.WindowCapture.Utils;
using System;
using System.Linq;
using System.Windows;

namespace Kzrnm.WindowCapture.Images
{
    public class ImageDropTarget : DefaultDropHandler
    {
        private readonly ImageProvider imageProvider;
        public bool AllowOtherDrop { get; }
        public ImageDropTarget(ImageProvider imageProvider) : this(imageProvider, false) { }
        public ImageDropTarget(ImageProvider imageProvider, bool allowOtherDrop)
        {
            this.imageProvider = imageProvider;
            this.AllowOtherDrop = allowOtherDrop;
        }
        public override void DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.VisualTarget == dropInfo.DragInfo?.VisualSource)
            {
                base.DragOver(dropInfo);
                return;
            }
            else if (dropInfo.Data is DataObject data)
            {
                if (data.GetData(DataFormats.FileDrop, false) is string[] files
                    && files.All(f => CaptureImageUtils.IsImageFile(f))
                    || data.ContainsImage())
                {
                    dropInfo.Effects = DragDropEffects.Copy;
                    if (dropInfo.TargetCollection == this.imageProvider.Images)
                        dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                    return;
                }
            }
            dropInfo.NotHandled = AllowOtherDrop;
        }

        public override void Drop(IDropInfo dropInfo)
        {
            if (dropInfo.VisualTarget == dropInfo.DragInfo?.VisualSource)
            {
                base.Drop(dropInfo);
                return;
            }
            else if (dropInfo.Data is DataObject data)
            {
                if (data.GetData(DataFormats.FileDrop, false) is string[] files)
                {
                    Array.Sort(files, NaturalComparer.Default);
                    if (dropInfo.InsertPosition == RelativeInsertPosition.None)
                        foreach (var file in files)
                            imageProvider.TryAddImageFromFile(file);
                    else
                        foreach (var file in files)
                            imageProvider.TryInsertImageFromFile(dropInfo.UnfilteredInsertIndex, file);
                    return;
                }
                if (data.ContainsImage())
                {
                    var img = data.GetImage();
                    if (dropInfo.InsertPosition == RelativeInsertPosition.None)
                        imageProvider.AddImage(img);
                    else
                        imageProvider.InsertImage(dropInfo.UnfilteredInsertIndex, img);
                    return;
                }
            }
            dropInfo.NotHandled = AllowOtherDrop;
        }
    }
}
