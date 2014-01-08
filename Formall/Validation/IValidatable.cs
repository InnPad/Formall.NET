using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Validation
{
    public interface IValidatable
    {
        IEnumerable<ValidationResult> Validate(ValidationContext validationContext);
    }
}
