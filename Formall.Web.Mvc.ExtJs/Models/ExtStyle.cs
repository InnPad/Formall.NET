using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Formall.Web.Mvc.ExtJs.Models
{
    /// <summary>
    /// Although this file only contains a variable, all vars are included by default
    /// in application sass builds, so this needs to be in the rule file section
    /// to allow javascript inclusion filtering to disable it.
    /// </summary>
    public class ExtStyle
    {
        public ExtStyle()
        {
            IncludeIE = false;
            IncludeRTL = false;
        }

        public bool IncludeIE
        {
            get;
            set;
        }

        /// <summary>
        /// {boolean} $include-rtl
        /// True to include right-to-left style rules.  This variable gets set to true automatically
        /// for rtl builds. You should not need to ever assign a value to this variable, however
        /// it can be used to suppress rtl-specific rules when they are not needed.  For example:
        /// @if (Model.IncludeRTL
        /// {
        ///     .x-rtl.foo {
        ///         margin-left: $margin-right;
        ///         margin-right: $margin-left;
        ///     }
        /// }
        /// </summary>
        public bool IncludeRTL
        {
            get;
            set;
        }

        public string Prefix
        {
            get;
            set;
        }

        public string BackgroundColor
        {
            get;
            set;
        }
    }
}