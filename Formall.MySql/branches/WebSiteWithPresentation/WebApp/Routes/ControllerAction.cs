using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Custom.Routes
{
    public class ControllerAction
    {
        public ActionNameAttribute Action
        {
            get;
            set;
        }

        public Controller Controller
        {
            get;
            set;
        }

        public string ControllerShortName
        {
            get;
            set;
        }

        public IEnumerable<Attribute> Attributes
        {
            get;
            set;
        }
    }
}