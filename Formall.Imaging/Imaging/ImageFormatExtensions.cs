using System;
using System.Windows.Media.Imaging;

namespace Formall.Imaging
{
    using Formall.Persistence;
    using Formall.Presentation;

    public static class ImageFormatExtensions
    {
        public static BitmapEncoder ChooseEncoder(this ImageFormat imageFormat)
        {
            switch (imageFormat)
            {
                case ImageFormat.Bmp:
                    return new BmpBitmapEncoder();
                case ImageFormat.Gif:
                    return new GifBitmapEncoder();
                case ImageFormat.Jpeg:
                    return new JpegBitmapEncoder();
                case ImageFormat.Png:
                    return new PngBitmapEncoder();
                case ImageFormat.Tiff:
                    return new TiffBitmapEncoder();
                case ImageFormat.Wmp:
                    return new WmpBitmapEncoder();
                default:
                    throw new ArgumentOutOfRangeException("imageFormat", imageFormat, "No such encoder support for this imageFormat");
            }
        }

        public static ImageFormat ToImageFormat(this MediaType mediaType)
        {
            if (MediaType.Bmp.Equals(mediaType))
                return ImageFormat.Bmp;

            if (MediaType.Gif.Equals(mediaType))
                return ImageFormat.Gif;

            if (MediaType.Jpeg.Equals(mediaType))
                return ImageFormat.Jpeg;

            if (MediaType.Png.Equals(mediaType))
                return ImageFormat.Png;

            if (MediaType.Tiff.Equals(mediaType))
                return ImageFormat.Tiff;

            if (MediaType.Wmp.Equals(mediaType))
                return ImageFormat.Wmp;

            return ImageFormat.Png;
        }

        public static string ToMediaType(this ImageFormat imageFormat)
        {
            switch (imageFormat)
            {
                case ImageFormat.Bmp:
                    return MediaType.Bmp;
                case ImageFormat.Gif:
                    return MediaType.Gif;
                case ImageFormat.Jpeg:
                    return MediaType.Jpeg;
                case ImageFormat.Png:
                    return MediaType.Png;
                case ImageFormat.Tiff:
                    return MediaType.Tiff;
                case ImageFormat.Wmp:
                    return MediaType.Wmp;
                default:
                    throw new ArgumentOutOfRangeException("imageFormat", imageFormat, "No such encoder support for this imageFormat");
            }
        }
    }
}
