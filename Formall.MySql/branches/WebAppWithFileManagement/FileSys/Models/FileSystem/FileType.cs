using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custom.Models.FileSystem
{
    public enum FileType : int
    {
        Folder = 0,

        #region - data -

        Data = 0x1000,

        Xml = Data | 1,

        #endregion - data -

        #region - document -

        Document = 0x2000,

        Pdf = Document | 1,

        #endregion - document -

        #region - image -

        Image = 0x1000,

        Ico = Image | 1,

        #endregion - image -

        #region - style -

        Style = 0x1000,

        Css = Style | 1,
        Xslt = Style | 2,

        #endregion - style -
    }
}
