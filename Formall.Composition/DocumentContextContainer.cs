using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall
{
    public class DocumentContextContainer
    {
        private CompositionContainer _container;

        public static DocumentContextContainer Current { get; set; }
    }
}
