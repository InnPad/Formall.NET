using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

namespace Custom.Areas.FileSystem.Controllers
{
    // upload:
    // http://superdit.com/2010/07/17/extjs-basic-multiple-file-upload/
    // http://jsjoy.com/blog/ext-js-extension-awesome-uploader
    // http://wtcindia.wordpress.com/2012/07/01/upload-files-using-awesome-uploader/

    using Extensions;
    using Models;

    public class FileController : ApiController
    {
        // GET api/values
        public object Get(string path)
        {
            return new { success = true, data = Helpers.PathHelper.GetFiles(path) };
        }

        // GET api/values/5
        public object Get(long id)
        {
            return new { success = true, data = "" };
        }

        // GET api/values/5
        public object Get(FileActionData data)
        {
            switch (data.Action)
            {
                case FileActions.DownloadFile:
                    break;
            }

            return new { success = true };
        }

        // POST api/values
        public object Post()
        {
            return Post(string.Empty);
        }

        // POST api/values
        public object Post(string path)
        {
            var context = HttpContext.Current;
            var request = context.Request;
            var headers = request.Headers;

            var statuses = new List<FilesStatus>();
            
            if (string.IsNullOrEmpty(headers["X-File-Name"]))
            {
                Helpers.UploadHelper.UploadWholeFile(context, statuses);
            }
            else
            {
                Helpers.UploadHelper.UploadPartialFile(headers["X-File-Name"], context, statuses);
            }

            return statuses;
            //Helpers.UploadHelper.WriteJsonIframeSafe(context, statuses);

            

            var list = new List<object>();

            foreach (string file in request.Files)
            {
                var hpf = request.Files[file] as HttpPostedFile;
                
                if (hpf.ContentLength == 0)
                    continue;

                string savedFileName = Path.Combine(
                   AppDomain.CurrentDomain.BaseDirectory, path ?? string.Empty,
                   Path.GetFileName(hpf.FileName));

                hpf.SaveAs(savedFileName);

                list.Add(new { success = true, name = savedFileName });
            }

            return new { success = true, context = list, result = list };

            // { success = false, error = ".json_encode($message)." }
        }

        // Download
        /*public HttpResponseMessage Post(string version, string environment, string filetype)
        {
            var path = @"C:\Temp\test.exe";
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(path, FileMode.Open);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            return result;
        }*/

        // PUT api/values/5
        public object Put([FromBody] FileActionData data)
        {
            Debug.Assert(data.Action == FileActions.MoveFile || data.Action == FileActions.RenameFile);

            var path = data.Path;

            var localPath = Helpers.PathHelper.PhysicalPath(path);

            var successful = new List<FileModel>();
            var failures = new List<FileModel>();

            foreach (var file in data.Files)
            {
                string fullName;
                if (Helpers.PathHelper.TryGetPath(file.Id, out fullName))
                {
                    var fi = new FileInfo(fullName);
                    if (fi.Exists)
                    {
                        string newName = localPath.Combine(file.Name);

                        if (File.Exists(newName))
                        {
                            if (data.Overwrite != true)
                            {
                                file.Message = "File already exists";
                                failures.Add(file);

                                continue;
                            }

                            File.Delete(newName);
                        }

                        try
                        {
                            fi.MoveTo(newName);
                            Helpers.PathHelper.MapPath(file.Id, newName);
                            successful.Add(file);
                        }
                        catch (Exception e)
                        {
                            file.Message = e.Message;
                            failures.Add(file);
                        }
                    }
                }
            }

            return new { success = true, data = new { successful = successful, failures = failures } };
        }

        // DELETE api/values/5
        public object Delete([FromBody] FileActionData data)
        {
            Debug.Assert(data.Action == FileActions.DeleteFile);

            var path = data.Path;

            var localPath = Helpers.PathHelper.PhysicalPath(path);

            var folderId = Helpers.PathHelper.GetFolderId(localPath);

            var successful = new List<FileModel>();

            foreach (var file in data.Files)
            {
                var fi2 = Helpers.PathHelper.GetInfoById(file.Id, localPath);
                var fi = new FileInfo(localPath.Combine(file.Name));
                if (fi.Exists)
                {
                    try
                    {
                        fi.Delete();

                        successful.Add(file);

                        file.Name = null;
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return new { success = true, data = new { successful = successful } };
        }
    }
}
