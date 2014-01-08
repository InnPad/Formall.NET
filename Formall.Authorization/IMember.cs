using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custom.Authorization
{
    public interface IMember
    {
        Profile Profile { get; }
    }
}
