using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Web.Mvc
{
    public static class Globals
    {
        public static string image_search_path { get { return ""; } }

        public static bool include_missing_images { get { return true; } }

        public static string theme_resource_path { get { return ""; } }
    }
}