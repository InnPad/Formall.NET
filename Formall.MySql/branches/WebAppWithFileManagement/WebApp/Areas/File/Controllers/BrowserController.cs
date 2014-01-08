using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Custom.Areas.File.Controllers
{
    public class BrowserController : Controller
    {
        //
        // GET: /File/Browser/

        public ActionResult Index()
        {
            return View();
        }

    }
}
