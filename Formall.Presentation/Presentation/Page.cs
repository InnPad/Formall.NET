using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;
using System.Xml;
using System.Xml.Linq;

namespace Formall.Presentation
{
    using Formall.Navigation;
    using Formall.Persistence;
    using Formall.Reflection;

    public class Page //: IDocument, IFileSystem, IHtml, ISegment, IXaml
    {
        private static Model _model;
        private IDocument _document;

        public string Layout
        {
            get;
            set;
        }

        public Content Content
        {
            get;
            set;
        }

        public string Model
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public Text Title
        {
            get;
            set;
        }
    }
}
