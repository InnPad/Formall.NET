using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Formall.Web.Mvc.Controllers
{
    using Formall.Imaging;

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

        protected ActionResult Render(BitMatrix matrix, ISizeCalculation size)
        {
            var renderer = new WriteableBitmapRenderer(size);

            DrawingSize dSize = size.GetSize(matrix == null ? 21 : matrix.Width);

            WriteableBitmap wBitmap = new WriteableBitmap(dSize.CodeWidth, dSize.CodeWidth, DPI.X, DPI.Y, PixelFormat, null);

            renderer.Draw(wBitmap, matrix);

            return Render(wBitmap, ImageFormat.Png);
        }

        protected ActionResult Render(DrawingVisual visual, ImageFormat imageFormat)
        {
            var width = (int)Math.Ceiling(visual.ContentBounds.Width);
            var height = (int)Math.Ceiling(visual.ContentBounds.Height);
            var bitmap = new RenderTargetBitmap(width, height, DPI.X, DPI.Y, PixelFormats.Pbgra32);
            bitmap.Render(visual);
            return Render(bitmap, imageFormat);
        }

        protected ImageResult Render(BitmapSource source, ImageFormat imageFormat)
        {
            var encoder = imageFormat.ChooseEncoder();
            encoder.Frames.Add(BitmapFrame.Create(source));
            return new ImageResult(encoder);
        }
    }
}
