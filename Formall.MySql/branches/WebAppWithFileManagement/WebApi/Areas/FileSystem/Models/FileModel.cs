using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Custom.Areas.FileSystem.Models
{
    public class FileModel
    {
        public ulong Id { get; set; }

        public string Name { get; set; }

        public long? Size { get; set; }

        public string Type { get; set; }

        public DateTime? Date { get; set; }

        public string Status { get; set; }

        public string row_class; // ux-unknown-file'

        public string Thumbnail; //this.defaultThumbnailImage}

        public string Message { get; set; }
    }
}