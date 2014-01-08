using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Custom.Areas.FileSystem.Models
{
    public class FolderActionData
    {
        public FolderActions Action { get; set; }
        public FolderModel Folder { get; set; }
        public string Path { get; set; }
    }
}