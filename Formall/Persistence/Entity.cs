using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Persistence
{
    public class Entity<T> where T : class
    {
        public static implicit operator T(Entity<T> entity)
        {
            return entity.Data;
        }

        public Guid Id
        {
            get;
            set;
        }

        public T Data
        {
            get;
            set;
        }
    }
}
