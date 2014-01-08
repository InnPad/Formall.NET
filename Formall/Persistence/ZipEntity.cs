using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Persistence
{
    using Formall.Presentation;
    using Formall.Reflection;

    public abstract class ZipEntity : ZipDocument, IEntity
    {
        private readonly ZipRepository _repository;

        protected ZipEntity(ZipArchiveEntry entry, MediaType type, Metadata metadata, ZipRepository repository)
            : base(entry, type, metadata, repository.Context)
        {
            _repository = repository;
        }

        public dynamic Data
        {
            get { return null; }
        }

        public Guid Id
        {
            get
            {
                Guid id;
                var key = Metadata.Key;
                Guid.TryParse(key.Split('/').Last(), out id);
                return id;
            }
        }

        public Model Model
        {
            get { return _repository.Model; }
        }

        public ZipRepository Repository
        {
            get { return _repository; }
        }

        IRepository IEntity.Repository
        {
            get { return _repository; }
        }

        public IResult Delete()
        {
            return this.Context.Delete(Key);
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
