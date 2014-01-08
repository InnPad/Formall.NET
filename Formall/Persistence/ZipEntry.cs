using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Persistence
{
    internal class ZipEntry
    {
        public string Name
        {
            get;
            set;
        }

        public Metadata Metadata
        {
            get;
            set;
        }
    }
}
