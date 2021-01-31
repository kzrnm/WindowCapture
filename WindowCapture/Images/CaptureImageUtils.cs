﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
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
        private static readonly Regex fileNamePattern
            = new Regex(@"\.(bmp|jpe?g|png)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        public static bool IsImageFile(ReadOnlySpan<char> fileName)
        {
            var ext = Path.GetExtension(fileName);
            if (ext.Length <= 1) return false;
            ext = ext[1..];
            return ext.Equals("jpeg", StringComparison.OrdinalIgnoreCase)
                || ext.Equals("jpg", StringComparison.OrdinalIgnoreCase)
                || ext.Equals("png", StringComparison.OrdinalIgnoreCase)
                || ext.Equals("bmp", StringComparison.OrdinalIgnoreCase);
        }
        public static readonly Regex jpegPattern
            = new Regex(@"\.jpe?g", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        public static bool IsJpegFile(ReadOnlySpan<char> fileName)
        {
            var ext = Path.GetExtension(fileName);
            if (ext.Length <= 1) return false;
            ext = ext[1..];
            return
                ext.Equals("jpg", StringComparison.OrdinalIgnoreCase)
                || ext.Equals("jpeg", StringComparison.OrdinalIgnoreCase);
        }
    }
}
