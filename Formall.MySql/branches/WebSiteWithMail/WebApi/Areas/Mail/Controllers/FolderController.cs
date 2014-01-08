using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Areas.Mail.Controllers
{
    public class FolderController : ApiController
    {
        // GET api/folder
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/folder/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/folder
        public void Post(string value)
        {
        }

        // PUT api/folder/5
        public void Put(int id, string value)
        {
        }

        // DELETE api/folder/5
        public void Delete(int id)
        {
        }
    }
}
