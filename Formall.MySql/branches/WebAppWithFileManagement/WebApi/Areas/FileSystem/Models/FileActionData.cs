using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Custom.Areas.FileSystem.Models
{
    public class FileActionData
    {
        public FileActions Action { get; set; }
        public string Path { get; set; }
        public FileModel[] Files { get; set; }
        public bool? Overwrite { get; set; }
    }
}