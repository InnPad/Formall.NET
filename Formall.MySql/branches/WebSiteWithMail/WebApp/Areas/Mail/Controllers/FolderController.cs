using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Custom.Areas.Mail.Controllers
{
    public class FolderController : Controller
    {
        //
        // GET: /Mail/Folder/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Inbox()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Inbox(int page)
        {
            return Json(null);
        }
    }
}
