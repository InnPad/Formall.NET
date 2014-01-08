using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall
{
    using Formall.Reflection;

    public class Program
    {
        public Set<Argument> Arguments { get; set; }

        public string Name { get; set; }

        public Prototype ResultType { get; set; }

        public ICollection<Variable> Variables { get; set; }

        /// <summary>
        /// Compressed Workflow
        /// </summary>
        public Binary Workflow { get; set; }
    }

    public abstract class ProgramWithResult : Program
    {
        /// <summary>
        /// When implemented in a derived class, gets or sets the value of an object
        /// </summary>
        public abstract object Result { get; set; }

        /// <summary>
        /// When implemented in a derived class, gets the type of the result.
        /// </summary>
        public abstract Type ResultType { get; }
    }

    public class Program<TResult> : ProgramWithResult
    {
        public override object Result
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override Type ResultType
        {
            get { return typeof(TResult); }
        }
    }
}
