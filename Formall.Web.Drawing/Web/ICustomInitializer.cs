using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Web
{
    public interface ICustomInitializer
    {
        void Initialize(object model, Action completed);
    }
}
