using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;

namespace Formall
{
    public interface IXaml
    {
        XamlMember ToXaml();
    }
}
