using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Diagnostics
{
    using Formall.Persistence;

    public class Logger : TraceListener
    {
        static Logger _instance;

        public static void Register()
        {
            Trace.Listeners.Add(_instance = new Logger
            {
                _repository = null,
                Filter = new LogFilter()
            });
        }

        public static void Unregister()
        {
            if (_instance != null)
            {
                Trace.Listeners.Remove(_instance);
                _instance.Dispose();
                _instance = null;
            }
        }

        private IRepository<EventTrace> _repository;

        private Logger()
        {
        }

        // Summary:
        //     When overridden in a derived class, closes the output stream so it no longer
        //     receives tracing or debugging output.
        public override void Close()
        {
            base.Close();
        }

        //
        // Summary:
        //     Releases the unmanaged resources used by the System.Diagnostics.TraceListener
        //     and optionally releases the managed resources.
        //
        // Parameters:
        //   disposing:
        //     true to release both managed and unmanaged resources; false to release only
        //     unmanaged resources.

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _repository.Dispose();
            }
        }

        //
        // Summary:
        //     Emits an error message to the listener you create when you implement the
        //     System.Diagnostics.TraceListener class.
        //
        // Parameters:
        //   message:
        //     A message to emit.
        public override void Fail(string message)
        {
        }

        //
        // Summary:
        //     Emits an error message and a detailed error message to the listener you create
        //     when you implement the System.Diagnostics.TraceListener class.
        //
        // Parameters:
        //   message:
        //     A message to emit.
        //
        //   detailMessage:
        //     A detailed message to emit.
        public override void Fail(string message, string detailMessage)
        {
        }

        //
        // Summary:
        //     When overridden in a derived class, flushes the output buffer.
        public override void Flush()
        {
            base.Flush();
        }

        //
        // Summary:
        //     Writes trace information, a data object and event information to the listener
        //     specific output.
        //
        // Parameters:
        //   eventCache:
        //     A System.Diagnostics.TraceEventCache object that contains the current process
        //     ID, thread ID, and stack trace information.
        //
        //   source:
        //     A name used to identify the output, typically the name of the application
        //     that generated the trace event.
        //
        //   eventType:
        //     One of the System.Diagnostics.TraceEventType values specifying the type of
        //     event that has caused the trace.
        //
        //   id:
        //     A numeric identifier for the event.
        //
        //   data:
        //     The trace data to emit.
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
        }

        //
        // Summary:
        //     Writes trace information, an array of data objects and event information
        //     to the listener specific output.
        //
        // Parameters:
        //   eventCache:
        //     A System.Diagnostics.TraceEventCache object that contains the current process
        //     ID, thread ID, and stack trace information.
        //
        //   source:
        //     A name used to identify the output, typically the name of the application
        //     that generated the trace event.
        //
        //   eventType:
        //     One of the System.Diagnostics.TraceEventType values specifying the type of
        //     event that has caused the trace.
        //
        //   id:
        //     A numeric identifier for the event.
        //
        //   data:
        //     An array of objects to emit as data.
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
        }

        //
        // Summary:
        //     Writes trace and event information to the listener specific output.
        //
        // Parameters:
        //   eventCache:
        //     A System.Diagnostics.TraceEventCache object that contains the current process
        //     ID, thread ID, and stack trace information.
        //
        //   source:
        //     A name used to identify the output, typically the name of the application
        //     that generated the trace event.
        //
        //   eventType:
        //     One of the System.Diagnostics.TraceEventType values specifying the type of
        //     event that has caused the trace.
        //
        //   id:
        //     A numeric identifier for the event.
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
        }

        //
        // Summary:
        //     Writes trace information, a message, and event information to the listener
        //     specific output.
        //
        // Parameters:
        //   eventCache:
        //     A System.Diagnostics.TraceEventCache object that contains the current process
        //     ID, thread ID, and stack trace information.
        //
        //   source:
        //     A name used to identify the output, typically the name of the application
        //     that generated the trace event.
        //
        //   eventType:
        //     One of the System.Diagnostics.TraceEventType values specifying the type of
        //     event that has caused the trace.
        //
        //   id:
        //     A numeric identifier for the event.
        //
        //   message:
        //     A message to write.
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
        }

        //
        // Summary:
        //     Writes trace information, a formatted array of objects and event information
        //     to the listener specific output.
        //
        // Parameters:
        //   eventCache:
        //     A System.Diagnostics.TraceEventCache object that contains the current process
        //     ID, thread ID, and stack trace information.
        //
        //   source:
        //     A name used to identify the output, typically the name of the application
        //     that generated the trace event.
        //
        //   eventType:
        //     One of the System.Diagnostics.TraceEventType values specifying the type of
        //     event that has caused the trace.
        //
        //   id:
        //     A numeric identifier for the event.
        //
        //   format:
        //     A format string that contains zero or more format items, which correspond
        //     to objects in the args array.
        //
        //   args:
        //     An object array containing zero or more objects to format.
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
        }

        //
        // Summary:
        //     Writes trace information, a message, a related activity identity and event
        //     information to the listener specific output.
        //
        // Parameters:
        //   eventCache:
        //     A System.Diagnostics.TraceEventCache object that contains the current process
        //     ID, thread ID, and stack trace information.
        //
        //   source:
        //     A name used to identify the output, typically the name of the application
        //     that generated the trace event.
        //
        //   id:
        //     A numeric identifier for the event.
        //
        //   message:
        //     A message to write.
        //
        //   relatedActivityId:
        //     A System.Guid object identifying a related activity.
        public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
        {
        }

        public override void Write(string message)
        {
            _repository.Add(Guid.NewGuid(), new EventTrace { EventType = TraceEventType.Verbose, Message = message });
        }

        public override void WriteLine(string message)
        {
            _repository.Add(Guid.NewGuid(), new EventTrace { EventType = TraceEventType.Verbose, Message = message });
        }
    }
}
