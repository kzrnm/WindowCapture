using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

namespace Kzrnm.WindowCapture.Images
{
    public static class CaptureImageUtils
    {
        /// <summary>
        /// ファイルから画像を読み込む
        /// 失敗したらnullを返す
        /// </summary>
        /// <param name="filePath">読み込むファイル</param>
        /// <returns></returns>
        public static CaptureImage? GetImageFromFile(string filePath)
        {
            using var ms = new MemoryStream(File.ReadAllBytes(filePath));
            try
            {
                var image = new WriteableBitmap(BitmapFrame.Create(ms));
                image.Freeze();
                return new CaptureImage(image, filePath);
            }
            catch (NotSupportedException e) when (e.InnerException is COMException)
            {
                return null;
            }
        }

    }
}
