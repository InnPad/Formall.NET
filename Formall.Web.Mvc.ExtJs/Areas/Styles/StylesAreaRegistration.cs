using System.Web.Mvc;

namespace Formall.Web.Mvc.ExtJs.Areas.Styles
{
    public class StylesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Styles";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Styles_default",
                "Styles/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
