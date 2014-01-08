using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Navigation
{
    using Formall.Persistence;
    using Formall.Presentation;
    using Formall.Reflection;
        
    internal class Document : Segment, IDocument, ISegment
    {
        private IDocument _document;
        private ISegment _parent;

        public Document(IDocument document, string name, Segment parent)
            : base(name, parent)
        {
            _document = document;
            _parent = parent;
        }

        public override SegmentClass Class
        {
            get { return SegmentClass.Document; }
        }

        protected Stream Content
        {
            get { return _document.Content; }
        }

        #region - IDocument -

        Stream IDocument.Content
        {
            get { return _document.Content; }
        }

        public Encoding ContentEncoding
        {
            get { return _document.ContentEncoding; }
        }

        public MediaType ContentType
        {
            get { return _document.ContentType; }
        }

        IDocumentContext IDocument.Context
        {
            get { return _document.Context; }
        }

        string IDocument.Key
        {
            get { return _document.Key; }
        }

        public Metadata Metadata
        {
            get { return _document.Metadata; }
        }

        #endregion - IDocument -
    }
}
