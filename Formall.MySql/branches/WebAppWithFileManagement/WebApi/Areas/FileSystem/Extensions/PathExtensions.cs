using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Custom.Areas.FileSystem.Extensions
{
    public static class PathExtensions
    {
        public static string Combine(this string path, string relative)
        {
            var pathWithEnding = path.EndsWith("\\") || path.EndsWith("/");
            var relativeWithBeginning = relative.EndsWith("\\") || relative.EndsWith("/");

            if (pathWithEnding)
                if (relative.StartsWith("\\"))
                    return path + relative.Substring(1);
                else if (relative.StartsWith("/"))
                    return path + relative.Substring(1);
                else
                    return path + relative;
            else
                if (relative.StartsWith("\\"))
                    return path + relative;
                else if (relative.StartsWith("/"))
                    return path + "\\" + relative.Substring(1);
                else
                    return path + "\\" + relative;
        }
    }
}