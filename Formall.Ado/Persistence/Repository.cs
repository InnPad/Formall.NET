using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Persistence
{
    internal class Repository : IRepository
    {
        public IDataContext Context
        {
            get { throw new NotImplementedException(); }
        }

        public Reflection.Model Model
        {
            get { throw new NotImplementedException(); }
        }

        public IEntity Create(object data)
        {
            throw new NotImplementedException();
        }

        public IResult Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEntity Read(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEntity[] Read(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public IResult Remove(Guid id, string field, string value)
        {
            throw new NotImplementedException();
        }

        public IResult Patch(Guid id, object data)
        {
            throw new NotImplementedException();
        }

        public IResult Update(Guid id, object data)
        {
            throw new NotImplementedException();
        }
    }
}
