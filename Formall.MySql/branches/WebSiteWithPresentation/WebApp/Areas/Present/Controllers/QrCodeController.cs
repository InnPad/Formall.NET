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
    using Algebra;
    using Algebra.QrCode.Encoding;
    using Presentation;

    public class QrCodeController : RenderController
    {
        public QrCodeController()
        {
            DPI = new Point(96, 96);
            PixelFormat = PixelFormats.Gray8;
        }

        //
        // GET: /Present/QrCode/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Text(string id, byte? moduleSize)
        {
            QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.M);
            QrCode qrCode = qrEncoder.Encode(id);
            return Render(qrCode.Matrix, new FixedModuleSize(moduleSize ?? 4, QuietZoneModules.Four));
        }

        private ActionResult Render(BitMatrix qrMatrix, ISizeCalculation size)
        {
            var renderer = new WriteableBitmapRenderer(size);

            DrawingSize dSize = size.GetSize(qrMatrix == null ? 21 : qrMatrix.Width);

            WriteableBitmap wBitmap = new WriteableBitmap(dSize.CodeWidth, dSize.CodeWidth, DPI.X, DPI.Y, PixelFormat, null);

            renderer.Draw(wBitmap, qrMatrix);

            return Render(wBitmap, ImageFormatEnum.PNG);
        }
    }
}
