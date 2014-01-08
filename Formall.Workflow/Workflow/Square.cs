using System;
using System.Activities;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Workflow
{
    class Square : Activity<int>
    {
        [RequiredArgument]
        public InArgument<int> Value { get; set; }

        public Square()
        {
            this.Implementation = () => new Sequence
            {
                Activities =
                    {
                        new WriteLine
                        {
                            Text = new InArgument<string>((env) => "Squaring the value: " + this.Value.Get(env))
                        },
                        new Assign<int>
                        {
                            To = new OutArgument<int>((env) => this.Result.Get(env)),
                            Value = new InArgument<int>((env) => this.Value.Get(env) * this.Value.Get(env))
                        }
                    }
            };
        }
    }
}
