using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
//using System.Web.Script.Serialization;

namespace Custom.Areas.FileSystem
{
    using Areas.FileSystem.Helpers;

    public class ContentHandler
    {
        private static ContentHandler _instance;

        public static ContentHandler Singlenton
        {
            get { return _instance ?? (_instance = new ContentHandler()); }
        }

        // Handle request based on method
        public static void HandleMethod(HttpContext context)
        {
            switch (context.Request.HttpMethod)
            {
                case "HEAD":
                case "GET":
                    if (GivenFilename(context)) Singlenton.DeliverFile(context);
                    else Singlenton.ListCurrentFiles(context);
                    break;

                case "POST":
                case "PUT":
                    Singlenton.UploadFile(context);
                    break;

                case "DELETE":
                    Singlenton.DeleteFile(context);
                    break;

                case "OPTIONS":
                    ReturnOptions(context);
                    break;

                default:
                    context.Response.ClearHeaders();
                    context.Response.StatusCode = 405;
                    break;
            }
        }

        //private readonly JavaScriptSerializer js;

        public ContentHandler()
        {
            /*js = new JavaScriptSerializer();
            js.MaxJsonLength = 41943040;*/
        }

        // Delete file from the server
        private void DeleteFile(HttpContext context)
        {
            var filePath = UploadHelper.StorageRoot + context.Request["f"];
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private static void ReturnOptions(HttpContext context)
        {
            context.Response.AddHeader("Allow", "DELETE,GET,HEAD,POST,PUT,OPTIONS");
            context.Response.StatusCode = 200;
        }

        // Upload file to the server
        private void UploadFile(HttpContext context)
        {
            var statuses = new List<FilesStatus>();
            var headers = context.Request.Headers;

            if (string.IsNullOrEmpty(headers["X-File-Name"]))
            {
                UploadHelper.UploadWholeFile(context, statuses);
            }
            else
            {
                UploadHelper.UploadPartialFile(headers["X-File-Name"], context, statuses);
            }

            UploadHelper.WriteJsonIframeSafe(context, statuses);
        }

        private static bool GivenFilename(HttpContext context)
        {
            return !string.IsNullOrEmpty(context.Request["f"]);
        }

        private void DeliverFile(HttpContext context)
        {
            var filename = context.Request["f"];
            var filePath = UploadHelper.StorageRoot + filename;

            if (File.Exists(filePath))
            {
                context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + filename + "\"");
                context.Response.ContentType = "application/octet-stream";
                context.Response.ClearContent();
                context.Response.WriteFile(filePath);
            }
            else
                context.Response.StatusCode = 404;
        }

        private void ListCurrentFiles(HttpContext context)
        {
            var files =
                new DirectoryInfo(UploadHelper.StorageRoot)
                    .GetFiles("*", SearchOption.TopDirectoryOnly)
                    .Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden))
                    .Select(f => new FilesStatus(f))
                    .ToArray();

            /*string jsonObj = js.Serialize(files);*/
            context.Response.AddHeader("Content-Disposition", "inline; filename=\"files.json\"");
            context.Response.Write(/*jsonObj*/null);
            context.Response.ContentType = "application/json";
        }
    }
}
