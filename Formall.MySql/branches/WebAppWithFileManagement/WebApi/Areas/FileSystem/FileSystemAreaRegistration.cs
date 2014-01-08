using System.Web.Mvc;

namespace Custom.Areas.FileSystem
{
    public class FileSystemAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "FileSystem";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "FileSystem_default",
                "FileSystem/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
