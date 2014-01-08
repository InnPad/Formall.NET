using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Persistence
{
    public class FileJsonEntity : FileEntity
    {
        public FileJsonEntity(string name, FileRepository repository)
            : base(name, repository)
        {
        }
    }
}
