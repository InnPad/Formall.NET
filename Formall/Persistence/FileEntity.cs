using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Persistence
{
    using Formall.Presentation;
    using Formall.Reflection;
    
    public abstract class FileEntity : FileDocument, IEntity
    {
        private readonly FileRepository _repository;

        protected FileEntity(string name, MediaType type, FileMetadata metadata, FileRepository repository)
            : base(name, type, metadata, repository.Context)
        {
            _repository = repository;
        }

        public abstract dynamic Data { get; }

        public Guid Id
        {
            get { throw new NotImplementedException(); }
        }

        public Model Model
        {
            get { throw new NotImplementedException(); }
        }

        public FileRepository Repository
        {
            get { return _repository; }
        }

        IRepository IEntity.Repository
        {
            get { return _repository; }
        }

        public IResult Delete()
        {
            throw new NotImplementedException();
        }

        public T Get<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public IResult Refresh()
        {
            throw new NotImplementedException();
        }

        public IResult Set<T>(T value) where T : class
        {
            throw new NotImplementedException();
        }

        IResult IEntity.Patch(object data)
        {
            throw new NotImplementedException();
        }

        IResult IEntity.Update(object data)
        {
            throw new NotImplementedException();
        }

        void IEntity.WriteJson(Stream stream)
        {
            throw new NotImplementedException();
        }

        void IEntity.WriteJson(TextWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
