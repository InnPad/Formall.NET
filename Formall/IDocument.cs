using System;
using System.IO;

namespace Formall
{
    using Formall.Persistence;
    using Formall.Presentation;
    using System.Text;

    public interface IDocument
    {
        Stream Content { get; }

        Encoding ContentEncoding { get; }

        MediaType ContentType { get; }

        IDocumentContext Context { get; }

        string Key { get; }

        Metadata Metadata { get; }
    }
}
