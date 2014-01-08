using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace System.Web.Helpers
{
    using Formall.Presentation;

    public static class InlineImage
    {
        public static IHtmlString InlineImageString(byte[] data,string mediaType)
        {
            //data = [data].flatten.pack('m').gsub("\n","")
            //url = "url('data:#{mime_type};base64,#{data}')"
            //Sass::Script::String.new(url)
            return new HtmlString(string.Empty);
        }
    }
}
