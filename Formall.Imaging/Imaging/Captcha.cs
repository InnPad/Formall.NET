using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;

namespace Formall.Imaging
{
    using Formall.Persistence;
    using Formall.Presentation;
    using System.Globalization;
    using System.IO;
    using System.Windows;
    using WPoint = System.Windows.Point;
    using WRect = System.Windows.Rect;
    using WSize = System.Windows.Size;

    public static class Captcha
    {
        private static readonly char[] _charArray = "ABCEFGHJKLMNPRSTUVWXYZ2346789".ToCharArray();

        private static readonly string[] FontFamilyNames = new[]
            {
                "Times New Roman",
                "Comic Sans MS",
                "Verdana"
            };

        public static string GenerateString(int length)
        {
            char[] captcha = new char[length];

            Random random = new Random();

            for (int x = 0; x < captcha.Length; x++)
            {
                captcha[x] = _charArray[random.Next(_charArray.Length)];
            }

            return new string(captcha);
        }

        public static byte[] GenerateImage(string text, ref MediaType mediaType, Color? foreground, Color? background, WSize? size, WSize? box, WPoint? origin)
        {
            byte[] result;

            var foreBrush = foreground.HasValue ? new SolidColorBrush(foreground.Value) : Brushes.Black;
            var backBrush = background.HasValue ? new SolidColorBrush(background.Value) : Brushes.Transparent;

            var bitmap = Print(text, foreBrush, backBrush, size ?? new Size(120, 80), box ?? new Size(160, 100), origin, new Point(96, 96));

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

            return result;
        }

        private static Typeface GenerateTypeface()
        {
            Random random = new Random();
            var index = random.Next(Fonts.SystemTypefaces.Count);
            return Fonts.SystemTypefaces.Skip(index).First();
        }

        private static BitmapSource Print(string text, Brush foreground, Brush background, WSize size, WSize box, WPoint? origin, WPoint dpi)
        {
            var visual = new DrawingVisual();

            var context = visual.RenderOpen();

            context.DrawRectangle(background, new Pen(background, 0.0), new WRect(new WPoint(0, 0), box));

            WPoint cursor = origin ?? new Point(0, 0);

            //var transform = new ScaleTransform(size.Width / originalSize.Width, size.Height / originalSize.Height, topLeft.X, topLeft.Y);
            //rectSize = new Size(size.Width / transform.ScaleX, size.Height / transform.ScaleY);
            //context.PushTransform(transform);

            foreach (var c in text)
            {
                var ft = new FormattedText(c.ToString(), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, GenerateTypeface(), size.Height, foreground);

                //context.PushOpacityMask(opacityMask);

                context.DrawText(ft, cursor);

                //context.Pop();

                cursor.X += ft.Width;
            }

            context.Close();

            var width = (int)Math.Ceiling(visual.ContentBounds.Width);
            var height = (int)Math.Ceiling(visual.ContentBounds.Height);
            var bitmap = new RenderTargetBitmap(width, height, dpi.X, dpi.Y, PixelFormats.Pbgra32);

            bitmap.Render(visual);

            return bitmap;
        }
    }
}
