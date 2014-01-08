using System;
using System.Activities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
//using System.Activities.Presentation.PropertyEditing;

namespace Formall.Workflow
{
    public sealed class Perform : CodeActivity
    {
        [Category("Input")]
        [DefaultValue(null)]
        public System.Int32? Identity { get; set; }

        //public Framework.Area Area { get; set; }

        //public Framework.Domain Domain { get; set; }

        //public Framework.Task Task { get; set; }

        public InArgument<bool> Required { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            //var type = Domain.GetRuntimeType();
        }

        private void OnFaulted(NativeActivityFaultContext faultContext, Exception propagatedException, ActivityInstance propagatedFrom)
        {
        }
    }
}
