using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Formall.Persistence
{
    using Formall.Reflection;

    public class DocumentContext : IDocumentContext
    {
        public IDocument Create()
        {
            throw new NotImplementedException();
        }

        public IResult Delete(string key)
        {
            throw new NotImplementedException();
        }

        public IDocument Import(Stream stream, MediaType type, Metadata metadata)
        {
            throw new NotImplementedException();
        }

        public IDocument Import(TextReader reader, MediaType type, Metadata metadata)
        {
            throw new NotImplementedException();
        }

        public IDocument Import(IDocument document)
        {
            throw new NotImplementedException();
        }

        public IDocument[] Read(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public IDocument Read(string key)
        {
            throw new NotImplementedException();
        }

        public IDocument[] Read(string keyPrefix, int skip, int take)
        {
            throw new NotImplementedException();
        }
    }
}
