using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Presentation
{
    using Formall.Navigation;
    using Formall.Persistence;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class SegmentOutput
    {
        private readonly ISegment _segment;
        private readonly bool _writeData;
        private readonly bool _writeMetadata;
        private readonly bool _deep;
        private readonly int _indentation;
        private readonly int _startIndent;

        public SegmentOutput(ISegment segment, bool writeData = true, bool writeMetadata = false, bool deep = false, int indentation = 0, int startIndent = 0)
        {
            _segment = segment;
            _writeData = writeData;
            _writeMetadata = writeMetadata;
            _deep = deep;
            _indentation = indentation;
            _startIndent = startIndent;
        }

        public void Write(TextWriter writer)
        {
            IDocument document;
            IEntity entity;

            var startIndent = _startIndent;
            writer.WriteLine(startIndent, "{");
            startIndent += _indentation;
            writer.WriteLine(startIndent, "success: true,");
            if (_deep || (_writeData && _writeMetadata))
            {
                writer.WriteLine(startIndent, "data: {");
                startIndent += _indentation;
                WriteDeep(writer, _segment.Path.TrimStart('/'), _segment, startIndent);
                startIndent -= _indentation;
                writer.WriteLine();
                writer.Write(startIndent, "}");
            }
            else if (_writeData && (entity = _segment as IEntity) != null)
            {
                WriteData(writer, entity, startIndent);
            }
            else if (_writeMetadata && (document = _segment as IDocument) != null)
            {
                WriteMetadata(writer, document, startIndent);
            }
            startIndent -= _indentation;
            writer.WriteLine();
            writer.Write(startIndent, "}");
        }

        private void WriteData(TextWriter writer, IEntity entity, int startIndent)
        {
            var sb = new StringBuilder();

            using (var sw = new StringWriter(sb))
            {
                entity.WriteJson(sw);
            }

            using (var sr = new StringReader(sb.ToString()))
            {
                writer.Write(startIndent, "data: ");

                var line = sr.ReadLine();
                writer.Write(line);

                for (line = sr.ReadLine(); !string.IsNullOrWhiteSpace(line); line = sr.ReadLine())
                {
                    writer.WriteLine();
                    writer.Write(startIndent, line);
                }
            }
        }

        private void WriteMetadata(TextWriter writer, IDocument document, int startIndent)
        {
            var sb = new StringBuilder();

            var serializer = new JsonSerializer
            {
                Formatting = _indentation > 0 ? Formatting.Indented : Formatting.None,
            };

            using (var sw = new StringWriter(sb))
            {
                serializer.Serialize(new JsonTextWriter(sw) { Indentation = _indentation }, JObject.FromObject(document.Metadata));
            }

            using (var sr = new StringReader(sb.ToString()))
            {
                writer.Write(startIndent, "metadata: ");

                var line = sr.ReadLine();
                writer.Write(line);

                for (line = sr.ReadLine(); !string.IsNullOrWhiteSpace(line); line = sr.ReadLine())
                {
                    writer.WriteLine();
                    writer.Write(startIndent, line);
                }
            }
        }

        private void WriteDeep(TextWriter writer, string key, ISegment segment, int startIndent)
        {
            IDocument document;
            IEntity entity;

            writer.WriteLine(startIndent, "\"" + key + "\": {");

            startIndent += _indentation;

            bool appendComma = false;

            if (_writeData & (entity = segment as IEntity) != null)
            {
                WriteData(writer, entity, startIndent);
                appendComma = true;
            }

            if (_writeMetadata && (document = segment as IDocument) != null)
            {
                if (appendComma)
                {
                    writer.WriteLine(",");
                }

                WriteMetadata(writer, document, startIndent);
                
                appendComma = true;
            }

            if (_deep)
            {
                foreach (var child in segment.Children)
                {
                    if (appendComma)
                    {
                        writer.WriteLine(",");
                    }

                    WriteDeep(writer, child.Value.Name, child.Value, startIndent);
                    appendComma = true;
                }
            }

            startIndent -= _indentation;

            writer.WriteLine();
            writer.Write(startIndent, "}");
        }
    }

    public static class TextWriterExtensions
    {
        public static void Write(this TextWriter writer, int indentation, string value)
        {
            writer.Write(new string(' ', indentation));
            writer.Write(value);
        }

        public static void Write(this TextWriter writer, int indentation, string format, params object[] args)
        {
            writer.Write(new string(' ', indentation));
            writer.Write(format, args);
        }

        public static void WriteLine(this TextWriter writer, int indentation, string value)
        {
            writer.Write(new string(' ', indentation));
            writer.WriteLine(value);
        }

        public static void WriteLine(this TextWriter writer, int indentation, string format, params object[] args)
        {
            writer.Write(new string(' ', indentation));
            writer.WriteLine(format, args);
        }
    }
}
