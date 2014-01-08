using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Persistence
{
    public class MediaType
    {
        public static MediaType Json = new MediaType("application/json");
        public static MediaType JavaScript = new MediaType("application/javascript");
        public static MediaType Binary = new MediaType("application/octet-stream");
        public static MediaType Ogg = new MediaType("application/ogg");
        public static MediaType Pdf = new MediaType("application/pdf");
        public static MediaType PostScript = new MediaType("application/postscript");
        public static MediaType Resource = new MediaType("application/rdf+xml");
        public static MediaType Rss = new MediaType("application/rss+xml");
        public static MediaType Soap = new MediaType("application/soap+xml");
        public static MediaType Font = new MediaType("application/font-woff");
        public static MediaType XHtml = new MediaType("application/xhtml+xml");
        public static MediaType Xml = new MediaType("application/xml");
        public static MediaType Dtd = new MediaType("application/xml-dtd");
        public static MediaType Xop = new MediaType("application/xop+xml");
        public static MediaType Zip = new MediaType("application/zip");
        public static MediaType GZip = new MediaType("application/gzip");
        //public static MediaType Font = new MediaType("application/x-font-woff");
        //public static MediaType JavaScript = new MediaType("application/x-javascript");

        public static MediaType Audio = new MediaType("audio/basic");
        public static MediaType L24 = new MediaType("audio/L24");
        //public static MediaType Mp3 = new MediaType("audio/mp4"); Mp4
        public static MediaType Mp3 = new MediaType("audio/mpeg");
        public static MediaType OggAudio = new MediaType("audio/ogg");
        public static MediaType Vorbis = new MediaType("audio/vorbis");
        public static MediaType RealAudio = new MediaType("audio/vnd.rn-realaudio");
        public static MediaType Wav = new MediaType("audio/vnd.wave");
        public static MediaType WebmAudio = new MediaType("audio/webm");

        public static MediaType Bmp = new MediaType("image/bmp", "Bitmap (BMP)");
        public static MediaType Gif = new MediaType("image/gif", "Graphic Interchange Format (GIF)");
        public static MediaType Jpeg = new MediaType("image/jpeg", "Joint Photographic Experts Group (JPEG)");
        //public static MediaType Jpeg = new MediaType("image/pjpeg");
        public static MediaType Png = new MediaType("image/png", "Portable Network Graphics (PNG)");
        public static MediaType Svg = new MediaType("image/svg+xml");
        public static MediaType Tiff = new MediaType("image/tiff", "Tagged Imag File Format (TIFF)");
        public static MediaType Icon = new MediaType("image/vnd.microsoft.icon");
        public static MediaType Wmp = new MediaType("image/vnd.ms-photo", "Microsoft Windows Media Photo");

        public static MediaType Email = new MediaType("multipart/mixed");
        //public static MediaType Email = new MediaType("multipart/alternative");
        //public static MediaType Email = new MediaType("multipart/related");
        public static MediaType Webform = new MediaType("multipart/form-data");
        public static MediaType Signed = new MediaType("multipart/signed");
        public static MediaType Encrypted = new MediaType("multipart/encrypted");

        public static MediaType Cmd = new MediaType("text/cmd");
        public static MediaType Css = new MediaType("text/css");
        public static MediaType Csv = new MediaType("text/csv");
        public static MediaType Html = new MediaType("text/html");
        //public static MediaType JavaScript = new MediaType("text/javascript");
        public static MediaType Text = new MediaType("text/plain");
        public static MediaType VCard = new MediaType("text/vcard");
        //public static MediaType Xml = new MediaType("text/xml");

        public static MediaType Mpeg = new MediaType("video/mpeg");
        public static MediaType Mp4 = new MediaType("video/mp4");
        public static MediaType OggVideo = new MediaType("video/ogg");
        public static MediaType QuickTime = new MediaType("video/quicktime");
        public static MediaType WebM = new MediaType("video/webm");
        public static MediaType Matroska = new MediaType("video/x-matroska");
        public static MediaType Wmv = new MediaType("video/x-ms-wmv");
        public static MediaType Flv = new MediaType("video/x-flv");

        private readonly string _value;

        private MediaType(string value, string description = null)
        {
            _value = value;
        }

        public static implicit operator string(MediaType mt)
        {
            return mt != null ? mt._value : null;
        }

        public static implicit operator MediaType(string value)
        {
            MediaType mt;

            value = value.ToLowerInvariant();
            
            if (!_dictionary.TryGetValue(value, out mt))
            {
                mt = _dictionary.AddOrUpdate(value, new MediaType(value), (key, existing) => { return existing; });
            }

            return mt;
        }

        private static readonly ConcurrentDictionary<string, MediaType> _dictionary = new ConcurrentDictionary<string, MediaType>(
            new Dictionary<string, MediaType>
            {
                { "application/json", Json },
                { "application/javascript", JavaScript },
                { "application/octet-stream", Binary },
                { "application/ogg", Ogg },
                { "application/pdf", Pdf },
                { "application/postscript", PostScript },
                { "application/rdf+xml", Resource },
                { "application/rss+xml", Rss },
                { "application/soap+xml", Soap },
                { "application/font-woff", Font },
                { "application/xhtml+xml", Html },
                { "application/xml", Xml },
                { "application/xml-dtd", Dtd },
                { "application/xop+xml", Xop },
                { "application/zip", Zip },
                { "application/gzip", GZip },
                { "application/x-font-woff", Font },
                { "application/x-javascript", JavaScript },

                { "audio/basic" , Audio },
                { "audio/L24", L24 },
                { "audio/mp4", Mp3 },
                { "audio/mpeg", Mp3 },
                { "audio/ogg", Ogg },
                { "audio/vorbis", Vorbis },
                { "audio/vnd.rn-realaudio", RealAudio },
                { "audio/vnd.wave", Wav },
                { "audio/webm", WebmAudio },

                { "image/bmp", Bmp },
                { "image/gif", Gif },
                { "image/jpeg", Jpeg },
                { "image/pjpeg", Jpeg },
                { "image/png", Png },
                { "image/svg+xml", Svg },
                { "image/tiff", Tiff },
                { "image/vnd.microsoft.icon", Icon },
                { "image/vnd.ms-photo", Wmp },

                { "multipart/mixed", Email },
                { "multipart/alternative", Email },
                { "multipart/related", Email },
                { "multipart/form-data", Webform },
                { "multipart/signed", Signed },
                { "multipart/encrypted", Encrypted },

                { "text/cmd", Cmd },
                { "text/css", Css },
                { "text/csv", Csv },
                { "text/html", Html },
                { "text/javascript", JavaScript },
                { "text/plain", Text },
                { "text/vcard", VCard },
                { "text/xml", Xml },

                { "video/mpeg", Mpeg },
                { "video/mp4", Mp4 },
                { "video/ogg", Ogg },
                { "video/quicktime", QuickTime },
                { "video/webm", WebM },
                { "video/x-matroska", Matroska },
                { "video/x-ms-wmv", Wmv },
                { "video/x-flv", Flv }
            });
    }
}
