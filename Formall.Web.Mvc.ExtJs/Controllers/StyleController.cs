using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Formall.Web.Mvc.ExtJs.Controllers
{
    public class StyleController : Controller
    {
        //
        // GET: /Style/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Ext()
        {
            var model = new Formall.Web.Mvc.ExtJs.Models.ExtStyle
            {
            };
            return View(model);
        }
    }
}
