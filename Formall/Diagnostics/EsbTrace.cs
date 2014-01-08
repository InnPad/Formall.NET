using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Diagnostics
{
    public static class EsbTrace
    {
        // Fields
        private static TraceSource tracking = new TraceSource("System.ServiceModel");
        private static TraceSource boot = null;

        static EsbTrace()
        {
            //Trace.AutoFlush = true;
            // tbd:
        }
        public static TraceSource Tracking
        {
            get { return tracking; }
        }
        public static TraceSource Boot
        {
            get { return tracking; }
        }


        [Conditional("TRACE")]
        public static void Event(TraceEventType eventType, int id)
        {
            Tracking.TraceEvent(eventType, id);
        }
        [Conditional("TRACE")]
        public static void Event(TraceEventType eventType, int id, string message)
        {
            Tracking.TraceEvent(eventType, id, message);
        }
        [Conditional("TRACE")]
        public static void Event(TraceEventType eventType, int id, string format, params object[] args)
        {
            Tracking.TraceEvent(eventType, id, format, args);
        }

        [Conditional("TRACE")]
        public static void Data(TraceEventType eventType, int id, params object[] data)
        {
            Tracking.TraceData(eventType, id, data);
        }
        [Conditional("TRACE")]
        public static void Data(TraceEventType eventType, int id, object data)
        {
            Tracking.TraceData(eventType, id, data);
        }

        [Conditional("TRACE")]
        public static void Information(string message)
        {
            Tracking.TraceInformation(message);
        }
        [Conditional("TRACE")]
        public static void Information(string format, params object[] args)
        {
            Tracking.TraceInformation(format, args);
        }
        [Conditional("TRACE")]
        public static void Transfer(int id, string message, Guid relatedActivityId)
        {
            Tracking.TraceTransfer(id, message, relatedActivityId);
        }

        [DebuggerStepThrough]
        public static string GetExceptionDetails(Exception ex)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Exception Information Type[{0}] Message[{1}] ", ex.GetType().Name, ex.Message);
            for (Exception exception = ex.InnerException; exception != null; exception = exception.InnerException)
            {
                builder.AppendFormat("Inner Exception Information Type[{0}] Message[{1}] ", exception.GetType().Name, exception.Message);
            }
            return builder.ToString();
        }
    }
}
