using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Diagnostics
{
    public class EventTrace
    {
        public int Id { get; set; }

        public TraceEventType EventType { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public string Message { get; set; }
    }
}
