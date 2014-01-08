using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Custom.Areas.FileSystem.Extensions;

namespace Custom.Areas.FileSystem.Controllers
{
    using Models;

    public class FolderController : ApiController
    {
        // GET api/datas
        public object Get(ulong id)
        {
            var relativePath = Helpers.ContentHelper.GetRelativePath(id);
            return new { success = true, data = Helpers.ContentHelper.QueryFolderAndChildFolders(relativePath).Children };
        }

        // GET api/datas/5
        /*public object Get(long id)
        {
            return "data";
        }*/

        // POST api/datas
        public object Post([FromBody]FolderActionData data)
        {
            Debug.Assert(data.Action == FolderActions.CreateFolder);

            var folder = Helpers.PathHelper.CreateFolder(data.Path);

            return new { success = true, data =  folder };
        }

        // PUT api/datas/5
        public object Put([FromBody]FolderActionData data)
        {
            /*Debug.Assert(data.Action == FolderActions.RenameFolder);

            Helpers.PathHelper.MoveFolder(data.Folder, data.Path);*/

            return new { success = true };
        }

        // DELETE api/datas/5
        public object Delete([FromBody]FolderActionData data)
        {
            Helpers.PathHelper.DeleteFolder(data.Folder);
            return new { success = true };
        }
    }
}
