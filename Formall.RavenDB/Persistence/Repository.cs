using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Persistence
{
    using Formall.Reflection;
    using Raven.Json.Linq;

    internal class Repository : IRepository
    {
        private readonly Model _model;
        private readonly string _keyPrefix;
        private readonly RavenDocumentContext _context;

        public Repository(string keyPrefix, Model model, RavenDocumentContext context)
        {
            _model = model;
            _keyPrefix = keyPrefix;
            _context = context;
        }

        public RavenDocumentContext Context
        {
            get { return _context; }
        }

        IDataContext IRepository.Context
        {
            get { return _context; }
        }

        public string KeyPrefix
        {
            get { return _keyPrefix; }
        }

        public Reflection.Model Model
        {
            get { return _model; }
        }

        public IEntity Create(object data)
        {
            var key = string.Empty;
            var metadata = RavenJObject.FromObject(new {
                @key = key,
                EntryType = Model.Name
            });
            var entity = new Entity(Guid.NewGuid(), RavenJObject.FromObject(data), metadata, this);

            using (var session = _context.DocumentStore.OpenSession())
            {
                _context.DocumentStore.DatabaseCommands.Put(key, null, entity.Data, entity.Metadata);
            }

            return entity;
        }

        public IResult Delete(Guid id)
        {
            _context.DocumentStore.DatabaseCommands.Delete(KeyPrefix + id, null);
            return null;
        }

        public IEntity Read(Guid id)
        {
            string key = KeyPrefix + id;
            var document = _context.DocumentStore.DatabaseCommands.Get(key);
            var entity = new Entity(id, document.DataAsJson, document.Metadata, this);
            return entity;
        }

        public IEntity[] Read(int skip, int take)
        {
            var documents = _context.DocumentStore.DatabaseCommands.StartsWith(KeyPrefix, null, skip, take);
            return documents.Select(o => new Entity(Entity.ParseId(o.Key), o.DataAsJson, o.Metadata, this)).OfType<IEntity>().ToArray();
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
