using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Formall.Persistence
{
    using Formall.Reflection;

    public interface IDataContext : IDocumentContext//, IValidationContext
    {
        IRepository CreateRepository(string name);

        IEntity Import(IEntity entity);
    }
}
