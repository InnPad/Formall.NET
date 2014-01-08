using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;
using Custom.Presentation;

namespace Custom.Areas.Present.Controllers
{
    using System.Windows.Media;
    using Custom.Presentation.Fractals;

    public class FractalController : RenderController
    {
        //
        // GET: /Present/Fractal/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChessBoard(byte? iterations, ushort? size)
        {
            var fractal = new ChessBoard(iterations ?? 3);
            var visual = fractal.CreateDrawingVisual(new Point(0, 0), new Size(size ?? 512, size ?? 512), 1.0, Brushes.Black, Brushes.Cyan);
            return Render(visual, ImageFormatEnum.PNG);
        }

        public ActionResult WhiteCollar(byte? iterations, ushort? size)
        {
            var fractal = new WhiteCollar(iterations ?? 3);
            var visual = fractal.CreateDrawingVisual(new Point(0, 0), new Size(size ?? 512, size ?? 512), 1.0, Brushes.Black, Brushes.Purple, Brushes.Green, Brushes.Red, Brushes.Cyan, Brushes.Yellow);
            return Render(visual, ImageFormatEnum.PNG);
        }

        public ActionResult Peano(byte? iterations, ushort? size)
        {
            var fractal = new PeanoCurve(iterations ?? 3);
            var visual = fractal.CreateDrawingVisual(new Point(0, 0), new Size(size ?? 512, size ?? 512), 1.0, Brushes.Black);
            return Render(visual, ImageFormatEnum.PNG);
        }

        public ActionResult Hilbert(byte? iterations, ushort? size)
        {
            var fractal = new HilbertCurve(iterations ?? 3);
            var visual = fractal.CreateDrawingVisual(new Point(0, 0), new Size(size ?? 512, size ?? 512), 1.0, Brushes.Black);
            return Render(visual, ImageFormatEnum.PNG);
        }

        public ActionResult Koch(byte? iterations, ushort? size)
        {
            var fractal = new KochSnowflake(iterations ?? 3);
            var visual = fractal.CreateDrawingVisual(new Point(0, 0), new Size(size ?? 512, size ?? 512), 1.0, Brushes.Black);
            return Render(visual, ImageFormatEnum.PNG);
        }

    }
}
