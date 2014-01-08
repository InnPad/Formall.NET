using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Navigation
{
    public interface ISegment
    {
        IDictionary<string, ISegment> Children { get; }

        string Name { get; }

        string Path { get; }

        ISegment Parent { get; }

        SegmentType Type { get; }
    }
}
