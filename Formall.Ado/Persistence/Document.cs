using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Persistence
{
    using Formall.Linq;
    using Formall.Reflection;
    
    internal class Document : IEntity
    {
        private readonly DataRow _dataRow;

        public Document(DataRow dataRow)
        {
            _dataRow = dataRow;
        }

        protected DataRow DataRow
        {
            get { return _dataRow; }
        }

        #region - Document -

        Stream IDocument.Content
        {
            get { throw new NotImplementedException(); }
        }

        IDocumentContext IDocument.Context
        {
            get { throw new NotImplementedException(); }
        }

        string IDocument.Key
        {
            get { throw new NotImplementedException(); }
        }

        Metadata IDocument.Metadata
        {
            get { throw new NotImplementedException(); }
        }

        #endregion - Document -

        public Stream Content
        {
            get { throw new NotImplementedException(); }
        }

        public Encoding ContentEncoding
        {
            get { return Encoding.Unicode; }
        }

        public MediaType ContentType
        {
            get { return MediaType.Xml; }
        }

        public dynamic Data
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Id
        {
            get { throw new NotImplementedException(); }
        }

        public Model Model
        {
            get { throw new NotImplementedException(); }
        }
        
        public IRepository Repository
        {
            get { throw new NotImplementedException(); }
        }

        public IResult Delete()
        {
            throw new NotImplementedException();
        }

        public T Get<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public IResult Refresh()
        {
            throw new NotImplementedException();
        }

        public IResult Set<T>(T value) where T : class
        {
            throw new NotImplementedException();
        }

        public IResult Patch(object data)
        {
            throw new NotImplementedException();
        }

        public IResult Update(object data)
        {
            throw new NotImplementedException();
        }

        Guid IEntity.Id
        {
            get { throw new NotImplementedException(); }
        }

        public void WriteJson(Stream stream)
        {
            throw new NotImplementedException();
        }

        public void WriteJson(TextWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
