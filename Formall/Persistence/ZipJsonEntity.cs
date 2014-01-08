using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Persistence
{
    public class ZipJsonEntity : ZipEntity
    {
        public ZipJsonEntity(ZipArchiveEntry entry, Metadata metadata, ZipRepository repository)
            : base(entry, metadata, repository)
        {
        }

        public override dynamic Data
        {
            get
            {
                return null;
            }
        }
    }
}
