using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Reflection
{
    using Formall.Validation;

    public static class FieldExtensions
    {
        public static IEnumerable<ValidationResult> Validate(this Field field, object value)
        {
            return new ValidationResult[0];
        }
    }
}
