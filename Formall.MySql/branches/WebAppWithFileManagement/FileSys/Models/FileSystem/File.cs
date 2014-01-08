using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custom.Models.FileSystem
{
    public class File
    {
        public int Identifier { get; set; }

        public uint FileIndexHigh { get; set; }

        public uint FileIndexLow { get; set; }

        public bool Folder { get; set; }

        public string Name { get; set; }

        public ulong Size { get; set; }

        public File Parent { get; set; }

        internal int ParentId { get; set; }

        public ICollection<File> Children { get; set; }

        public FileType Type { get; set; }
    }
}
