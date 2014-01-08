using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Workflow
{
    /// <summary>
    /// An activity is represented with a rounded-corner rectangle and describes the kind of work which must be done.
    /// </summary>
    public enum ActivityClass
    {
        /// <summary>
        /// Used to hide or reveal additional levels of business process detail. When collapsed, a sub-process is indicated by a plus sign against the bottom line of the rectangle; when expanded, the rounded rectangle expands to show all flow objects, connecting objects, and artifacts.
        /// Has its own self-contained start and end events; sequence flows from the parent process must not cross the boundary.
        /// </summary>
        Process,

        /// <summary>
        /// A task represents a single unit of work that is not or cannot be broken down to a further level of business process detail without diagramming the steps in a procedure (which is not the purpose of BPMN)
        /// </summary>
        Task,

        /// <summary>
        /// A form of sub-process in which all contained activities must be treated as a whole; i.e., they must all be completed to meet an objective, and if any one of them fails, they must all be compensated (undone). Transactions are differentiated from expanded sub-processes by being surrounded by a double border.
        /// </summary>
        Transaction
    }
}
