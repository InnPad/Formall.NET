using System;
using System.IO;
using System.Web.Mvc;
using System.Windows.Media.Imaging;

namespace Formall.Web.Mvc
{
    using Formall.Presentation;

    public class ImageResult : BinaryResult
    {
        /// <summary>
        /// Initializes a new instance of the Empress.Results.ImageResult class.
        /// </summary>
        internal ImageResult(BitmapEncoder encoder)
        {
            if (encoder == null)
            {
                throw new ArgumentNullException("encoder");
            }

            ContentType = encoder.CodecInfo.MimeTypes;

            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);
                Content = stream.ToArray();
            }
        }
    }
}