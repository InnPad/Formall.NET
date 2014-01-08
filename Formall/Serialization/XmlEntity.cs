using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Serialization
{
    using Formall.Persistence;
    using Formall.Presentation;

    public class XmlEntity : IEntity
    {
        public static XmlEntity Clone(IEntity entity)
        {
            return null;
        }

        dynamic IEntity.Data
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        Guid IEntity.Id
        {
            get { throw new NotImplementedException(); }
        }

        Reflection.Model IEntity.Model
        {
            get { throw new NotImplementedException(); }
        }

        IRepository IEntity.Repository
        {
            get { throw new NotImplementedException(); }
        }

        IResult IEntity.Delete()
        {
            throw new NotImplementedException();
        }

        T IEntity.Get<T>()
        {
            throw new NotImplementedException();
        }

        IResult IEntity.Refresh()
        {
            throw new NotImplementedException();
        }

        IResult IEntity.Set<T>(T value)
        {
            throw new NotImplementedException();
        }

        IResult IEntity.Patch(object data)
        {
            throw new NotImplementedException();
        }

        IResult IEntity.Update(object data)
        {
            throw new NotImplementedException();
        }

        void IEntity.WriteJson(System.IO.Stream stream)
        {
            throw new NotImplementedException();
        }

        void IEntity.WriteJson(System.IO.TextWriter writer)
        {
            throw new NotImplementedException();
        }

        System.IO.Stream IDocument.Content
        {
            get { throw new NotImplementedException(); }
        }

        public Encoding ContentEncoding
        {
            get { return Encoding.Unicode; }
        }

        IDocumentContext IDocument.Context
        {
            get { throw new NotImplementedException(); }
        }

        MediaType IDocument.ContentType
        {
            get { return MediaType.Xml; }
        }

        string IDocument.Key
        {
            get { throw new NotImplementedException(); }
        }

        Metadata IDocument.Metadata
        {
            get { throw new NotImplementedException(); }
        }
    }
}
