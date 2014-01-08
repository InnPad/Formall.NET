using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Web.Hepers
{
    using System.Runtime.CompilerServices;
    
    public class GridColumn
    {
        [Dynamic(new bool[] { false, true, false }), CompilerGenerated]
        private Func<object, object> __BackingField;
        
        public bool CanSort { get; set; }
        
        public string ColumnName { get; set; }
        
        [Dynamic(new bool[] { false, true, false })]
        public Func<object, object> Format
        {
            [return: Dynamic(new bool[] { false, true, false })]
            get;
            [param: Dynamic(new bool[] { false, true, false })]
            set;
        }
        
        public string Header { get; set; }
        
        public string Style { get; set; }
    }
}