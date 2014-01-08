using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Persistence
{
    using Formall.Presentation;

    public class FileDocumentContext : IDataContext
    {
        public static implicit operator DirectoryInfo(FileDocumentContext context)
        {
            return context != null ? context._directory : null;
        }

        private FileIndex _index;
        private readonly DirectoryInfo _directory;

        public FileDocumentContext(DirectoryInfo directory)
        {
            _directory = directory;
            _index = new FileIndex(this);
        }

        public FileIndex Index
        {
            get { return _index; }
        }

        public DirectoryInfo Directory
        {
            get { return _directory; }
        }

        public FileRepository CreateRepository(string name)
        {
            throw new NotImplementedException();
        }

        public FileDocument Import(Stream stream, MediaType type, Metadata metadata)
        {
            throw new NotImplementedException();
        }

        public FileDocument Import(TextReader reader, MediaType type, Metadata metadata)
        {
            throw new NotImplementedException();
        }

        public FileDocument Import(IDocument document)
        {
            throw new NotImplementedException();
        }

        public FileEntity Import(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public FileDocument Read(string key)
        {
            var entry = _index[key];

            return entry != null ? new FileDocument(entry.Name, MediaType.Binary, entry.Metadata, this) : null;
        }

        public FileDocument[] Read(int skip, int take)
        {
            return _index.Select(entry => new FileDocument(entry, this)).Skip(skip).Take(take).ToArray();
        }

        public FileDocument[] Read(string keyPrefix, int skip, int take)
        {
            var keys = _index.Keys.Where(o => o.StartsWith(keyPrefix)).ToArray();

            return keys.Select(key => new FileDocument(_index[key], this)).Skip(skip).Take(take).ToArray();
        }

        public void Refresh()
        {
            if (_directory.Exists)
            {
                var files = _directory.GetFiles("*.*", SearchOption.AllDirectories);

                var index = new FileIndex(this);

                var root = _directory.FullName;

                foreach (var file in files)
                {
                    var name = file.FullName;

                    if (file.FullName.StartsWith(root))
                    {
                        name = name.Substring(root.Length);
                    }

                    name = name.Replace('\\', '/').TrimStart('/');

                    var ext = file.Extension.ToLowerInvariant();

                    if (!string.IsNullOrEmpty(ext))
                    {
                        name = name.Substring(0, name.Length - ext.Length);
                    }

                    var key = name.Replace('-', '/');
                    
                    var metadata = new FileMetadata
                    {
                        Extension = ext,
                        Key = key,
                        Private = false
                    };

                    MediaType mt;
                    if (!FileExtensionToMediaType.TryGetValue(ext.TrimStart('.'), out mt))
                    {
                        mt = MediaType.Binary;
                    }

                    metadata.MediaType = mt;

                    var old = _index.Get(name);

                    if (old != null)
                    {
                        metadata.Private = old.Metadata.Private;
                    }

                    index.Set(name, metadata);
                }

                index.Save();

                _index = index;
            }
        }

        #region - IDocumentContext -

        IResult IDocumentContext.Delete(string key)
        {
            throw new NotImplementedException();
        }

        IDocument IDocumentContext.Import(Stream stream, MediaType type, Metadata metadata)
        {
            return Import(stream, type, metadata);
        }

        IDocument IDocumentContext.Import(TextReader reader, MediaType type, Metadata metadata)
        {
            return Import(reader, type, metadata);
        }

        IDocument IDocumentContext.Import(IDocument document)
        {
            return Import(document);
        }

        IDocument IDocumentContext.Read(string key)
        {
            return Read(key);
        }

        IDocument[] IDocumentContext.Read(int skip, int take)
        {
            return Read(skip, take);
        }

        IDocument[] IDocumentContext.Read(string keyPrefix, int skip, int take)
        {
            return Read(keyPrefix, skip, take);
        }

        #endregion - IDocumentContext -

        #region - IDataContext -

        IRepository IDataContext.CreateRepository(string name)
        {
            return CreateRepository(name);
        }

        IEntity IDataContext.Import(IEntity entity)
        {
            return Import(entity);
        }

        #endregion - IDataContext -

        static readonly Dictionary<string, MediaType> FileExtensionToMediaType = new Dictionary<string, MediaType>
        {
            //{ "ogg", "application/ogg" },
            //{ "ps", MediaType.PostScript },
            //{ "rdf", "application/rdf+xml" },
            //{ "soap", "application/soap+xml" },
            //{ "font", "application/font-woff" },
            
            //{ "dtd", "application/xml-dtd" },
            //{ "xop", "application/xop+xml" },
            //{ "x-font", "application/x-font-woff" },
            //{ "audio", "audio/basic" },
            //{ "l24", "audio/L24" },
            //{ "ogg", "audio/ogg" },
            //{ "vorbis", "audio/vorbis" },
            //{ "realaudio", "audio/vnd.rn-realaudio" },
            //{ "webm", "audio/webm" },
            
            { "cmd", MediaType.Cmd },
            { "css", MediaType.Css },
            { "csv", MediaType.Csv },
            { "ico", MediaType.Icon },
            { "icon", MediaType.Icon },
            { "html", MediaType.Html },
            { "jpg", MediaType.Jpeg },
            { "jpeg", MediaType.Jpeg },
            { "js", MediaType.JavaScript },
            { "json", MediaType.Json },
            { "flv", MediaType.Flv },
            { "gif", MediaType.Gif },
            { "gzip", MediaType.GZip },
            { "mkv", MediaType.Matroska },
            { "mpeg", MediaType.Mpeg },
            { "mp3", MediaType.Mp3 },
            { "mp4", MediaType.Mp4 },
            { "mov", MediaType.QuickTime },
            { "pdf", MediaType.Pdf },
            { "png", MediaType.Png },
            { "rss", MediaType.Rss },
            { "svg", MediaType.Svg },
            { "tiff", MediaType.Tiff },
            { "txt", MediaType.Text },
            { "vcard", MediaType.VCard },
            { "wave", MediaType.Audio },
            { "webm", MediaType.WebM },
            { "wmv", MediaType.Wmv },
            { "xhtml", MediaType.XHtml },
            { "xml", MediaType.Xml },
            { "zip", MediaType.Zip }
        };
    }
}
