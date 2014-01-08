using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Persistence
{
    using Formall.Serialization;

    public class ZipXmlEntity : ZipEntity
    {
        public ZipXmlEntity(ZipArchiveEntry entry, Metadata metadata, ZipRepository repository)
            : base(entry, metadata, repository)
        {
        }

        public override dynamic Data
        {
            get
            {
                var entity = new JsonEntity(Content, Metadata);
                return entity.Data;
            }
        }
    }
}
