//
// For licensing info goto <http://mvcxaml.codeplex.com/license>

using System;
using System.Globalization;
using System.Text;
using System.Web.Mvc;

namespace Formall.Web.Mvc
{
    using Formall.Imaging;

    public class XamlImageActionResult : ViewResultBase
    {
        public new object Model { get; private set; }

        public XamlImageActionResult(string xamlViewName)
            : this(xamlViewName, null)
        { }

        public XamlImageActionResult(string xamlViewName, ImageFormat imageFormat)
            : this(xamlViewName, null, imageFormat)
        { }


        public XamlImageActionResult(string xamlViewName, object model)
            : this(xamlViewName, model, ImageFormat.Png)
        { }

        public XamlImageActionResult(string xamlViewName, object model, ImageFormat imageFormat)
        {
            if (xamlViewName == null)
            {
                throw new ArgumentNullException("xamlViewName");
            }

            base.ViewName = xamlViewName;
            this.Model = model;
        }

        protected override ViewEngineResult FindView(ControllerContext context)
        {
            ViewEngineResult result = base.ViewEngineCollection.FindView(context, base.ViewName, null);
            if (result.View != null)
            {
                return result;
            }
            StringBuilder builder = new StringBuilder();
            foreach (string str in result.SearchedLocations)
            {
                builder.AppendLine();
                builder.Append(str);
            }
            throw new InvalidOperationException(
                string.Format(
                    CultureInfo.CurrentUICulture,
                    "The view '{0}' or its master was not found. The following locations were searched:{1}",
                    new object[] { base.ViewName, builder }
                    )
                );
        }
    }
}
