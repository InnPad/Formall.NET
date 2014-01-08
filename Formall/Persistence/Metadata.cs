using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Persistence
{
    public class Metadata
    {
        public virtual string Key { get; set; }

        public virtual bool Private { get; set; }

        /// <summary>
        /// {IDocumentContext.Version.V1}.{Change.Number}
        /// Examples: V1, V1.1, V1.2, V1.3, V2
        /// </summary>
        public virtual string Version { get; set; }
    }
}
