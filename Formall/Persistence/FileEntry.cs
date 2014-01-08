using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Persistence
{
    public class FileEntry
    {
        public string Name
        {
            get;
            set;
        }

        public FileMetadata Metadata
        {
            get;
            set;
        }
    }
}
