using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using System.Web.UI;

namespace Custom.Routes
{
    public class SimpleRequestHandler : Page, IHttpAsyncHandler, IRequiresSessionState
    {
        public SimpleRequestHandler(RequestContext requestContext)
        {
            if (requestContext == null)
                throw new ArgumentNullException("requestContext");
            this.RequestContext = requestContext;
        }

        private ControllerBuilder _controllerBuilder;

        internal ControllerBuilder ControllerBuilder
        {
            get { return this._controllerBuilder ?? (this._controllerBuilder = ControllerBuilder.Current); }
        }

        public RequestContext RequestContext { get; set; }

        protected override void OnInit(EventArgs e)
        {
            string requiredString = this.RequestContext.RouteData.GetRequiredString("controller");
            var controllerFactory = this.ControllerBuilder.GetControllerFactory();
            var controller = controllerFactory.CreateController(this.RequestContext, requiredString);
            if (controller == null)
                throw new InvalidOperationException("Could not find controller: " + requiredString);
            try
            {
                controller.Execute(this.RequestContext);
            }
            finally
            {
                controllerFactory.ReleaseController(controller);
            }
            this.Context.ApplicationInstance.CompleteRequest();
        }

        public override void ProcessRequest(HttpContext httpContext)
        {
            throw new NotSupportedException("This should not get called for an STA");
        }

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            return this.AspCompatBeginProcessRequest(context, cb, extraData);
        }

        public void EndProcessRequest(IAsyncResult result)
        {
            this.AspCompatEndProcessRequest(result);
        }

        void IHttpHandler.ProcessRequest(HttpContext httpContext)
        {
            this.ProcessRequest(httpContext);
        }
    }
}