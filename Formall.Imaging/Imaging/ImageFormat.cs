using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Imaging
{
    public enum ImageFormat : int
    {
        /// <summary>
        /// Portable Network Graphics (PNG)
        /// </summary>
        Png = 0,
        /// <summary>
        /// Joint Photographic Experts Group (JPEG)
        /// </summary>
        Jpeg = 1,
        /// <summary>
        /// Bitmap (BMP)
        /// </summary>
        Bmp = 2,
        /// <summary>
        /// Graphic Interchange Format (GIF)
        /// </summary>
        Gif = 3,
        /// <summary>
        /// Tagged Imag File Format (TIFF)
        /// </summary>
        Tiff = 4,
        /// <summary>
        /// Microsoft Windows Media Photo
        /// </summary>
        Wmp = 5
    }
}
