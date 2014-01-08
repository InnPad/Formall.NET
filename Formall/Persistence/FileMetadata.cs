using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Persistence
{
    public class FileMetadata : Metadata
    {
        public string Extension { get; set; }

        public string MediaType { get; set; }
    }
}
