using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace Formall.Navigation
{
    using Formall.Navigation;
    using Formall.Persistence;
    using Formall.Reflection;

    /// <summary>
    /// Root Segment
    /// </summary>
    public class Domain
    {
        /// <summary>
        /// Default culture
        /// </summary>
        public string Culture
        {
            get;
            set;
        }

        public string Pattern
        {
            get;
            set;
        }
    }
}
