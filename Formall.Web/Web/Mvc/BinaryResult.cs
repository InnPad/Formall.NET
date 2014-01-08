using System;
using System.IO;
using System.Text;
using System.Web.Mvc;

namespace Formall.Web.Mvc
{
    using Formall.Presentation;
    using System.Web;
    
    public class BinaryResult : ActionResult
    {
        public virtual byte[] Content { get; set; }

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

            var buffer = this.Content;
            if (buffer != null)
            {
                response.Clear();
                response.BinaryWrite(buffer);
            }
        }
    }
}
