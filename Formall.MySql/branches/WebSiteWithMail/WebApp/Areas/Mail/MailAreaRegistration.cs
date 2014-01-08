using System.Web.Mvc;

namespace Custom.Areas.Mail
{
    public class MailAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Mail";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Mail_default",
                "Mail/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
