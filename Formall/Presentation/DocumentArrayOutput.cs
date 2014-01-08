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

    public class DocumentArrayOutput
    {
        private readonly IEnumerable<IDocument> _documents;
        private readonly bool _writeData;
        private readonly bool _writeMetadata;
        private readonly int _indentation;
        private readonly int _startIndent;

        public DocumentArrayOutput(IEnumerable<IDocument> documents, bool writeData = true, bool writeMetadata = false, int indentation = 0, int startIndent = 0)
        {
            _documents = documents;
            _writeData = writeData;
            _writeMetadata = writeMetadata;
            _indentation = indentation;
            _startIndent = startIndent;
        }

        public void Write(TextWriter writer)
        {
            var startIndent = _startIndent;
            writer.WriteLine(startIndent, "{");
            startIndent += _indentation;
            writer.WriteLine(startIndent, "success: true,");
            writer.WriteLine(startIndent, "data: [");
            startIndent += _indentation;
            bool appendComma = false;
            if (_writeData)
            {
                foreach (var entity in _documents.OfType<IEntity>())
                {
                    if (appendComma)
                    {
                        writer.WriteLine(",");
                    }

                    writer.Write(startIndent, string.Empty);
                    WriteData(writer, entity, startIndent);

                    appendComma = true;
                }
            }
            else if (_writeMetadata)
            {
                foreach (var document in _documents)
                {
                    if (appendComma)
                    {
                        writer.WriteLine(",");
                    }

                    writer.Write(startIndent, string.Empty);
                    WriteMetadata(writer, document, startIndent);

                    appendComma = true;
                }
            }

            startIndent -= _indentation;

            if (appendComma)
            {
                writer.WriteLine();
                writer.Write(startIndent, "]");
            }
            else
            {
                writer.Write("]");
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
                var line = sr.ReadLine();
                writer.Write(line);

                for (line = sr.ReadLine(); !string.IsNullOrWhiteSpace(line); line = sr.ReadLine())
                {
                    writer.WriteLine();
                    writer.Write(startIndent, line);
                }
            }
        }
    }
}
