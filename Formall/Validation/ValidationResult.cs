using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Validation
{
    public class ValidationResult : IResult
    {
        public static readonly ValidationResult Success;

        private readonly Error _error;

        public ValidationResult(Error error)
        {
            _error = error;
        }
        
        public bool IsAsynchronous
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsSuccess
        {
            get { throw new NotImplementedException(); }
        }

        public void Write(Stream stream)
        {
            throw new NotImplementedException();
        }

        public void Write(TextWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
