using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Custom.Areas.FileSystem.Models
{
    public class FolderModel
    {
        public ulong Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public ICollection<FolderModel> Children { get; set; }
    }
}