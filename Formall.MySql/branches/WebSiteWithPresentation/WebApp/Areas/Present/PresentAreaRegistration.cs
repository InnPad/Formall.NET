using System.Web.Mvc;
using System.Web.Routing;

namespace Custom.Areas.Present
{
    using Routes;

    public class PresentAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Present";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.Add(new Route("Present/{controller}/{action}", new SimpleRouteHandler()));

            context.MapRoute(
                "Present_default",
                "Present/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
