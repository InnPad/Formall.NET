// Copyright (c) 2011 Chris Pietschmann <http://pietschsoft.com>
//
// This file is part of MvcXaml <http://mvcxaml.codeplex.com>
//
// For licensing info goto <http://mvcxaml.codeplex.com/license>

using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Formall.Imaging
{
    public class RenderHelper
    {
        /// <summary>
        /// Renders the given control to a PNG image
        /// </summary>
        /// <param name="xamlControl">The control to render</param>
        /// <returns></returns>
        public static MemoryStream RenderControlAsImageStream(FrameworkElement xamlControl)
        {
            return RenderControlAsImageStream(xamlControl, ImageFormat.Png);
        }

        /// <summary>
        /// Renders the given control to an image using the specified BitmapEncoder
        /// </summary>
        /// <param name="xamlControl">The control to render</param>
        /// <param name="encoder">The BitmapEncoder to use to encode the image</param>
        /// <returns></returns>
        public static MemoryStream RenderControlAsImageStream(FrameworkElement xamlControl, ImageFormat imageFormat)
        {
            var bitmap = RenderControlAsBitmap(xamlControl);

            // Encode image and output to Response.OutputStream
            BitmapEncoder encoder = GetBitmapEncoder(imageFormat);
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            var ms = new MemoryStream();
            encoder.Save(ms);
            return ms;
        }

        /// <summary>
        /// Renders the given control to a RenderTargetBitmap object. This method is used by internally within RenderControlAsImageStream
        /// </summary>
        /// <param name="xamlControl">The control to render</param>
        /// <returns></returns>
        public static RenderTargetBitmap RenderControlAsBitmap(FrameworkElement xamlControl)
        {
            // Get control size
            var controlSize = new Size(xamlControl.Width, xamlControl.Height);

            // If control Width is Zero or NaN, then default to 400
            if (controlSize.Width == 0 || Double.IsNaN(controlSize.Width))
            {
                controlSize.Width = 400;
            }
            // If control Height is Zero or NaN, then default to 400
            if (controlSize.Height == 0 || Double.IsNaN(controlSize.Height))
            {
                controlSize.Height = 400;
            }

            xamlControl.Measure(controlSize);
            xamlControl.Arrange(new Rect(controlSize));

            xamlControl.UpdateLayout();

            var bitmap = new RenderTargetBitmap((int)controlSize.Width, (int)controlSize.Height, 96, 96, PixelFormats.Pbgra32);

            var visual = new DrawingVisual();
            using (var drawingContext = visual.RenderOpen())
            {
                var brush = new VisualBrush(xamlControl);
                drawingContext.DrawRectangle(brush, null, new Rect(controlSize));
            }
            visual.Transform = new ScaleTransform((int)controlSize.Width / controlSize.Width, (int)controlSize.Height / controlSize.Height);
            bitmap.Render(visual);

            return bitmap;
        }


        private static BitmapEncoder GetBitmapEncoder(ImageFormat imageFormat)
        {
            switch (imageFormat)
            {
                case ImageFormat.Png:
                    return new PngBitmapEncoder();
                case ImageFormat.Jpeg:
                    return new JpegBitmapEncoder();
                case ImageFormat.Bmp:
                    return new BmpBitmapEncoder();
                case ImageFormat.Gif:
                    return new GifBitmapEncoder();
                case ImageFormat.Tiff:
                    return new TiffBitmapEncoder();
                case ImageFormat.Wmp:
                    return new WmpBitmapEncoder();
                default:
                    throw new NotSupportedException(string.Format("The specified ImageFormat ({0}) is Unsupported.", imageFormat));
            }
        }
    }
}
