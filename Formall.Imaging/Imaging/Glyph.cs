using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Formall.Imaging
{
    using Formall.Persistence;
    using Formall.Presentation;
    using WPoint = System.Windows.Point;
    using WRect = System.Windows.Rect;
    using WSize = System.Windows.Size;

    public static class Glyph
    {
        public static byte[] Transform(byte[] source, ref MediaType mediaType, string foregroundColor, string backgroundColor, string inner, string outter, string originPoint)
        {
            var foreground = GetColor(foregroundColor);
            var background = GetColor(backgroundColor);

            var size = GetSize(inner);
            var box = GetSize(outter);
            var origin = GetPoint(originPoint);

            return Transform(source, ref mediaType, foreground, background, size, box, origin);
        }

        public static byte[] Transform(byte[] source, ref MediaType mediaType, NameValueCollection parameters)
        {
            var foreground = GetColor(parameters["foreground"]);
            var background = GetColor(parameters["background"]);

            var size = GetSize(parameters["size"]);
            var box = GetSize(parameters["box"]) ?? size;
            var origin = GetPoint(parameters["origin"]);

            return Transform(source, ref mediaType, foreground, background, size, box, origin);
        }

        internal static byte[] Transform(byte[] source, ref MediaType mediaType, Color? foreground, Color? background, WSize? size, WSize? box, WPoint? origin)
        {
            byte[] result = source;

            if (foreground.HasValue || background.HasValue || size.HasValue || box.HasValue)
            {
                BitmapImage image = Image(source);

                var originalSize = new WSize(image.Width, image.Height);

                if (foreground.HasValue || background.HasValue || (size.HasValue && Different(size.Value, originalSize)) || (box.HasValue && Different(box.Value, originalSize)))
                {
                    var foreBrush = foreground.HasValue ? new SolidColorBrush(foreground.Value) : Brushes.Black;
                    var backBrush = background.HasValue ? new SolidColorBrush(background.Value) : Brushes.Transparent;
                    
                    var bitmap = Print(image, foreBrush, backBrush, size ?? originalSize, box ?? size ?? originalSize, origin, new WPoint(96, 96));

                    var imageFormat = mediaType.ToImageFormat();

                    var encoder = imageFormat.ChooseEncoder();
                    
                    encoder.Frames.Add(BitmapFrame.Create(bitmap));

                    mediaType = imageFormat.ToMediaType();

                    using (var stream = new MemoryStream())
                    {
                        encoder.Save(stream);
                        stream.Seek(0, SeekOrigin.Begin);
                        result = stream.ToArray();
                    }
                }
            }

            return result;
        }

        private static BitmapSource Print(ImageSource image, Brush foreground, Brush background, WSize size, WSize box, WPoint? origin, WPoint dpi)
        {
            var visual = new DrawingVisual();

            var context = visual.RenderOpen();

            context.DrawRectangle(background, new Pen(background, 0.0), new WRect(new WPoint(0, 0), box));

            #region mask

            var originalSize = new WSize(image.Width, image.Height);

            var opacityMask = new ImageBrush(image);

            bool scale = Different(size, originalSize);

            var topLeft = origin ?? Center(size, box);

            var rectSize = size;

            if (scale)
            {
                var transform = new ScaleTransform(size.Width / originalSize.Width, size.Height / originalSize.Height, topLeft.X, topLeft.Y);
                rectSize = new Size(size.Width / transform.ScaleX, size.Height / transform.ScaleY);
                context.PushTransform(transform);
            }

            context.PushOpacityMask(opacityMask);

            context.DrawRectangle(foreground, new Pen(foreground, 0.0), new System.Windows.Rect(topLeft, rectSize));

            context.Pop();

            if (scale)
            {
                context.Pop();
            }

            #endregion

            context.Close();

            var width = (int)Math.Ceiling(visual.ContentBounds.Width);
            var height = (int)Math.Ceiling(visual.ContentBounds.Height);
            var bitmap = new RenderTargetBitmap(width, height, dpi.X, dpi.Y, PixelFormats.Pbgra32);
            
            bitmap.Render(visual);

            return bitmap;
        }

        private static BitmapImage Image(byte[] buffer)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new System.IO.MemoryStream(buffer);
            image.EndInit();
            return image;
        }


        #region = Helpers =

        public static WPoint Center(WSize inner, WSize outter)
        {
            return new WPoint(Math.Floor((outter.Width - inner.Width) / 2), Math.Floor((outter.Height - inner.Height) / 2));
        }

        private static bool Different(WSize left, WSize right)
        {
            return (int)Math.Ceiling(left.Height) != (int)Math.Ceiling(right.Height) && (int)Math.Ceiling(left.Width) != (int)Math.Ceiling(right.Width);
        }

        private static Int32Array? GetArray(string source)
        {
            var array = Int32Array.Parse(source);
            return array.Length > 0 ? (Int32Array?)array : null;
        }

        private static Color? GetColor(string source)
        {
            byte luminance, alpha, red, green, blue;

            if (string.IsNullOrEmpty(source) || source.ToLowerInvariant().ToCharArray().Any(o => "0123456789abcdef".IndexOf(o) < 0))
            {
                return null;
            }

            switch (source.Length)
            {
                case 1:
                    luminance = byte.Parse(new string(source[0], 2), NumberStyles.HexNumber);
                    return Color.FromRgb(luminance, luminance, luminance);

                case 2:
                    luminance = byte.Parse(source, NumberStyles.HexNumber);
                    return Color.FromRgb(luminance, luminance, luminance);

                case 3:
                    red = byte.Parse(new string(source[0], 2), NumberStyles.HexNumber);
                    green = byte.Parse(new string(source[1], 2), NumberStyles.HexNumber);
                    blue = byte.Parse(new string(source[2], 2), NumberStyles.HexNumber);
                    return Color.FromRgb(red, green, blue);

                case 4:
                    alpha = byte.Parse(new string(source[0], 2), NumberStyles.HexNumber);
                    red = byte.Parse(new string(source[1], 2), NumberStyles.HexNumber);
                    green = byte.Parse(new string(source[2], 2), NumberStyles.HexNumber);
                    blue = byte.Parse(new string(source[3], 2), NumberStyles.HexNumber);
                    return Color.FromArgb(alpha, red, green, blue);

                case 5:
                    alpha = byte.Parse(source.Substring(0, 2));
                    red = byte.Parse(new string(source[2], 2), NumberStyles.HexNumber);
                    green = byte.Parse(new string(source[3], 2), NumberStyles.HexNumber);
                    blue = byte.Parse(new string(source[4], 2), NumberStyles.HexNumber);
                    return Color.FromArgb(alpha, red, green, blue);

                case 6:
                    red = byte.Parse(source.Substring(0, 2), NumberStyles.HexNumber);
                    green = byte.Parse(source.Substring(2, 2), NumberStyles.HexNumber);
                    blue = byte.Parse(source.Substring(4, 2), NumberStyles.HexNumber);
                    return Color.FromRgb(red, green, blue);

                case 7:
                    alpha = byte.Parse(new string(source[0], 2), NumberStyles.HexNumber);
                    red = byte.Parse(source.Substring(1, 2), NumberStyles.HexNumber);
                    green = byte.Parse(source.Substring(3, 2), NumberStyles.HexNumber);
                    blue = byte.Parse(source.Substring(5, 2), NumberStyles.HexNumber);
                    return Color.FromArgb(alpha, red, green, blue);

                case 8:
                    alpha = byte.Parse(source.Substring(0, 2), NumberStyles.HexNumber);
                    red = byte.Parse(source.Substring(2, 2), NumberStyles.HexNumber);
                    green = byte.Parse(source.Substring(4, 2), NumberStyles.HexNumber);
                    blue = byte.Parse(source.Substring(6, 2), NumberStyles.HexNumber);
                    return Color.FromArgb(alpha, red, green, blue);
            }

            return null;
        }

        private static WPoint? GetPoint(string source)
        {
            var array = Int32Array.Parse(source);
            return array.Length > 0 ? (WPoint?)array : null;
        }

        private static WRect? GetRect(string source)
        {
            var array = Int32Array.Parse(source);
            return array.Length.Equals(2) || array.Length.Equals(4) ? (WRect?)array : null;
        }

        private static WSize? GetSize(string source)
        {
            var array = Int32Array.Parse(source);
            return array.Length > 0 ? (WSize?)array : null;
        }

        private static bool TryGetArray(string source, out Int32Array array)
        {
            array = Int32Array.Parse(source);
            return array.Length > 0;
        }

        public static bool TryGetArray(this NameValueCollection collection, string name, out Int32Array array)
        {
            return TryGetArray(collection[name], out array);
        }

        private static bool TryGetColor(string source, out Color color)
        {
            var nullable = GetColor(source);
            color = nullable ?? System.Windows.Media.Colors.Transparent;
            return nullable.HasValue;
        }

        internal static bool TryGetColor(this NameValueCollection collection, string name, out Color color)
        {
            return TryGetColor(collection[name], out color);
        }

        private static bool TryGetPoint(string source, out WPoint point)
        {
            var array = Int32Array.Parse(source);
            point = (WPoint)array;
            return array.Length > 0;
        }

        internal static bool TryGetPoint(this NameValueCollection collection, string name, out WPoint point)
        {
            return TryGetPoint(collection[name], out point);
        }

        private static bool TryGetRect(string source, out WRect rect)
        {
            var array = Int32Array.Parse(source);
            rect = (WRect)array;
            return array.Length.Equals(4);
        }

        internal static bool TryGetRect(this NameValueCollection collection, string name, out WRect rect)
        {
            return TryGetRect(collection[name], out rect);
        }

        private static bool TryGetSize(string source, out WSize size)
        {
            var array = Int32Array.Parse(source);
            size = (WSize)array;
            return array.Length > 0;
        }

        internal static bool TryGetSize(this NameValueCollection collection, string name, out WSize size)
        {
            return TryGetSize(collection[name], out size);
        }

        #endregion = Helpers =
    }
}
