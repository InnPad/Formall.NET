using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Persistence
{
    using Formall.Presentation;

    public class ZipDocument : IDocument
    {
        public static implicit operator ZipArchiveEntry(ZipDocument document)
        {
            return document != null ? document._entry : null;
        }

        private readonly ZipDocumentContext _context;
        private readonly ZipArchiveEntry _entry;
        private readonly Metadata _metadata;
        private MediaType _type;

        public ZipDocument(ZipArchiveEntry entry, MediaType type, Metadata metadata, ZipDocumentContext context)
        {
            _entry = entry;
            _type = type;
            _metadata = metadata;
            _context = context;
        }

        public Stream Content
        {
            get { return _entry != null ? _entry.Open() : null; }
        }

        public IDocumentContext Context
        {
            get { return _context; }
        }

        public Encoding ContentEncoding
        {
            get { return Encoding.Unicode; }
        }

        public MediaType ContentType
        {
            get { return _type; }
        }

        public string Key
        {
            get { return _metadata != null ? _metadata.Key : null; }
        }

        public Metadata Metadata
        {
            get { return _metadata; }
        }

        public IResult Save()
        {

            return null;
        }
    }
}
