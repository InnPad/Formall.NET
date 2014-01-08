using System;
using System.IO;
using System.Web.Mvc;
using System.Windows.Media.Imaging;

namespace Custom.Results
{
    public class ImageResult : ActionResult
    {
        BitmapEncoder _encoder;

        /// <summary>
        /// Initializes a new instance of the Empress.Results.ImageResult class.
        /// </summary>
        public ImageResult(BitmapEncoder encoder)
        {
            if (encoder == null)
            {
                throw new ArgumentNullException("encoder");
            }

            _encoder = encoder;
        }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that
        /// inherits from the System.Web.Mvc.ActionResult class.
        /// </summary>
        /// <param name="context">
        /// The context in which the result is executed. The context information includes
        /// the controller, HTTP content, request context, and route data.
        /// </param>
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.ContentType = _encoder.CodecInfo.MimeTypes;

            using (var mem = new MemoryStream())
            {
                _encoder.Save(mem);
                mem.Seek(0, SeekOrigin.Begin);
                var buffer = mem.ToArray();

                context.HttpContext.Response.OutputStream.Write(buffer, 0, buffer.Length);
            }
        }
    }
}