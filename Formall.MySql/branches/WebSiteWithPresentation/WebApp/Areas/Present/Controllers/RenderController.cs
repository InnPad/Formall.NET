using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Custom.Areas.Present.Controllers
{
    using Custom.Algebra.QrCode.Encoding;
    using Custom.Presentation;
    using Custom.Results;

    public abstract class RenderController : Controller
    {
        protected RenderController()
        {
            DPI = new Point(72.0, 72.0);
            PixelFormat = PixelFormats.Pbgra32;
        }

        protected PixelFormat PixelFormat
        {
            get;
            set;
        }

        protected Point DPI
        {
            get;
            set;
        }

        protected ActionResult Render(DrawingVisual visual, ImageFormatEnum imageFormat)
        {
            var width = (int)Math.Ceiling(visual.ContentBounds.Width);
            var height = (int)Math.Ceiling(visual.ContentBounds.Height);
            var bitmap = new RenderTargetBitmap(width, height, DPI.X, DPI.Y, PixelFormats.Pbgra32);
            bitmap.Render(visual);
            return Render(bitmap, imageFormat);
        }

        protected Results.ImageResult Render(BitmapSource source, ImageFormatEnum imageFormat)
        {
            var encoder = imageFormat.ChooseEncoder();
            encoder.Frames.Add(BitmapFrame.Create(source));
            return new ImageResult(encoder);
        }
    }
}
