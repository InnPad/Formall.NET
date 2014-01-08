using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Persistence
{
    using Formall.Reflection;

    public class FileRepository : IRepository
    {
        private readonly Model _model;
        private readonly FileDocumentContext _context;

        public FileRepository(Model model, FileDocumentContext context)
        {
            _model = model;
            _context = context;
        }

        public FileDocumentContext Context
        {
            get { return _context; }
        }

        IDataContext IRepository.Context
        {
            get { return _context; }
        }

        public Model Model
        {
            get { return _model; }
        }

        IEntity IRepository.Create(object data)
        {
            throw new NotImplementedException();
        }

        IResult IRepository.Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        IEntity IRepository.Read(Guid id)
        {
            throw new NotImplementedException();
        }

        IEntity[] IRepository.Read(int skip, int take)
        {
            throw new NotImplementedException();
        }

        IResult IRepository.Remove(Guid id, string field, string value)
        {
            throw new NotImplementedException();
        }

        IResult IRepository.Patch(Guid id, object data)
        {
            throw new NotImplementedException();
        }

        IResult IRepository.Update(Guid id, object data)
        {
            throw new NotImplementedException();
        }
    }
}
