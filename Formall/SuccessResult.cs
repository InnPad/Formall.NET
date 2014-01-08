using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall
{
    public class SuccessResult : OperationResult
    {
        public SuccessResult()
            : base(false)
        {
        }

        public object Data
        {
            get;
            set;
        }

        public override void Write(Stream stream)
        {
            throw new NotImplementedException();
        }

        public override void Write(TextWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
