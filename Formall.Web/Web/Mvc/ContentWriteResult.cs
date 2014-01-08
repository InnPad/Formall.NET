using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Formall.Web.Mvc
{
    public class ContentWriteResult : ActionResult
    {
        public virtual Action<TextWriter> Write { get; set; }

        public virtual Encoding ContentEncoding { get; set; }

        public virtual string ContentType { get; set; }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that
        /// inherits from the System.Web.Mvc.ActionResult class.
        /// </summary>
        /// <param name="context">
        /// The context in which the result is executed. The context information includes
        /// the controller, HTTP content, request context, and route data.
        /// </param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;
            if (!string.IsNullOrEmpty(this.ContentType))
            {
                response.ContentType = this.ContentType;
            }

            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }

            var write = this.Write;
            if (write != null)
            {
                response.Clear();
                write(response.Output);
            }
        }
    }
}
