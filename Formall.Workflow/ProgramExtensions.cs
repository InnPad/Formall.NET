using System;
using System.Activities;
using System.Activities.XamlIntegration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xaml;

namespace Formall
{
    using Formall.Persistence;
    using Formall.Persistence;
    using Formall.Workflow;
    
    public static class ProgramExtensions
    {
        public static IDictionary<string, object> Execute(this Program program)
        {
            Activity activity = new Square();
            Dictionary<string, object> inputs = new Dictionary<string, object> { { "Value", 5 } };
            return WorkflowInvoker.Invoke(activity, inputs);
        }

        public static TResult Execute<TResult>(this Program<TResult> program)
        {
            Activity<TResult> activity = null;
            Dictionary<string, object> inputs = new Dictionary<string, object> { { "Value", 5 } };
            return WorkflowInvoker.Invoke(activity, inputs);
        }

        public static Workflow.Program Load(this Program program, Guid id, IRepository<Program> repo)
        {
            if (program.Workflow == null)
            {
                var entity = repo.Select.Include(o => o.Workflow).SingleOrDefault(o => o.Id == id);
                program.Workflow = entity.Data.Workflow;
            }

            var xaml = program.Workflow.Decompress().Decode();

            // Create a new ActivityBuilder and initialize it using the serialized workflow definition.
            /*ActivityBuilder*/
            var activityBuilder = XamlServices.Load(ActivityXamlServices.CreateBuilderReader(new XamlXmlReader(new StringReader(xaml))));

            return new Workflow.Program
            {
                DisplayName = program.Name,
            };
        }
    }
}
