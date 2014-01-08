using System.Web.Mvc;

namespace Custom.Areas.File
{
    public class FileAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "File";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "File_default",
                "File/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
