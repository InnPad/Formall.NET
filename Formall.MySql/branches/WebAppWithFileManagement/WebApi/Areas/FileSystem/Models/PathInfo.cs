using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Custom.Areas.FileSystem.Models
{
    public class PathInfo
    {
        public PathInfo(string name, PathInfo parent)
        {
            Name = name;
            Parent = parent;
        }

        public string Name;

        public PathInfo Parent;

        public string FullPath
        {
            get { return Parent != null ? Parent.FullPath + "/" + Name : "~/" + Name; }
        }
    }
}