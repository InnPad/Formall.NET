using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;
using System.Windows.Media;

namespace Formall.Web.Mvc.Controllers
{
    using Formall.Imaging;
    using Formall.Imaging.Fractals;
    using Formall.Persistence;
    using Formall.Presentation;
    using System.Globalization;
    using System.Windows.Media.Imaging;

    public class ImageController : RenderController
    {
        private static readonly object _lock = new object();
        private static FileDocumentContext _context;

        public ImageController()
        {
            DPI = new Point(96, 96);
            PixelFormat = System.Windows.Media.PixelFormats.Gray8;
        }

        public static FileDocumentContext Context
        {
            get
            {
                var context = _context;

                if (context == null)
                {
                    lock (_lock)
                    {
                        context = _context ?? (_context = new FileDocumentContext(new System.IO.DirectoryInfo(System.Web.Hosting.HostingEnvironment.MapPath("~/Images/Glyph"))));
                    }
                }

                return context;
            }
        }

        public ActionResult Index()
        {
            Context.Refresh();
            return View(Context);
        }

        public ActionResult Glyph(string id, string foreground, string background, string inner, string outter, string origin)
        {
            var prefix = string.IsNullOrEmpty(id) ? string.Empty : id.Replace('-', '/');

            var matches = Context.Read(prefix, 0, 100);

            if (matches == null || matches.Length.Equals(0))
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            else if (matches.Length.Equals(1))
            {
                var file = matches[0];
                var stream = System.IO.File.Open(file.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read);

                if (!stream.CanRead)
                {
                    stream.Close();
                }

                stream.Seek(0L, System.IO.SeekOrigin.Begin);

                Byte[] buffer = new Byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                MediaType mediaType = file.Metadata.MediaType;

                var content = Imaging.Glyph.Transform(buffer, ref mediaType, Request.Params);

                stream.Close();

                return new BinaryResult { Content = content, ContentType = mediaType };
            }
            else
            {
                return Json(new { success = true, matches = matches.Select(o => new { name = o.Name, ext = o.Metadata.Extension, type = o.Metadata.MediaType }) }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Refresh()
        {
            string message = null;

            try
            {
                Context.Refresh();
            }
            catch (Exception e)
            {
                message = e.Message;
            }

            return Json(message == null ? (object)new { success = true } : new { success = false, message = message }, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Image/QrCode

        public ActionResult QrCode(string contents, byte? moduleSize)
        {
            BitMatrix matrix = Formall.Imaging.Barcode.QrCode.Encode(contents, 1, 1);
            return Render(matrix, new FixedModuleSize(moduleSize ?? 4, QuietZoneModules.Four));
        }

        public ActionResult Barcode(string contents, byte? moduleSize, int? width, int? height, string format)
        {
            BitMatrix matrix = Formall.Imaging.Barcode.Encode(contents, width ?? 1, height ?? 1, format);
            return Render(matrix, new FixedModuleSize(moduleSize ?? 4, QuietZoneModules.Four));
        }

        public ActionResult ChessBoard(byte? iterations, ushort? size)
        {
            var fractal = new ChessBoard(iterations ?? 3);
            var visual = fractal.CreateDrawingVisual(new Point(0, 0), new Size(size ?? 512, size ?? 512), 1.0, Brushes.Black, Brushes.Cyan);
            return Render(visual, ImageFormat.Png);
        }

        public ActionResult WhiteCollar(byte? iterations, ushort? size)
        {
            var fractal = new WhiteCollar(iterations ?? 3);
            var visual = fractal.CreateDrawingVisual(new Point(0, 0), new Size(size ?? 512, size ?? 512), 1.0, Brushes.Black, Brushes.Purple, Brushes.Green, Brushes.Red, Brushes.Cyan, Brushes.Yellow);
            return Render(visual, ImageFormat.Png);
        }

        public ActionResult Peano(byte? iterations, ushort? size)
        {
            var fractal = new PeanoCurve(iterations ?? 3);
            var visual = fractal.CreateDrawingVisual(new Point(0, 0), new Size(size ?? 512, size ?? 512), 1.0, Brushes.Black);
            return Render(visual, ImageFormat.Png);
        }

        public ActionResult Hilbert(byte? iterations, ushort? size)
        {
            var fractal = new HilbertCurve(iterations ?? 3);
            var visual = fractal.CreateDrawingVisual(new Point(0, 0), new Size(size ?? 512, size ?? 512), 1.0, Brushes.Black);
            return Render(visual, ImageFormat.Png);
        }

        public ActionResult Koch(byte? iterations, ushort? size)
        {
            var fractal = new KochSnowflake(iterations ?? 3);
            var visual = fractal.CreateDrawingVisual(new Point(0, 0), new Size(size ?? 512, size ?? 512), 1.0, Brushes.Black);
            return Render(visual, ImageFormat.Png);
        }
    }
}
