using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Navigation
{
    using Formall.Persistence;
    using Formall.Reflection;
    using Formall.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;
    
    internal class Entity : Document, IEntity
    {
        private readonly IEntity _entity;
        private IDataContext _context;

        public Entity(IEntity entity, string name, Segment parent)
            : base(entity, name, parent)
        {
            _entity = entity;
        }

        public override SegmentClass Class
        {
            get { return SegmentClass.Entity; }
        }

        public IDataContext Context
        {
            get { return _context ?? (_context = _entity.Repository.Context); }
            set { _context = value; }
        }

        public dynamic Data
        {
            get;
            set;
        }

        public Guid Id
        {
            get { return _entity.Id; }
        }

        public Model Model
        {
            get { return _entity.Model; }
        }

        IRepository IEntity.Repository
        {
            get { return _entity.Repository; }
        }

        IResult IEntity.Delete()
        {
            throw new NotImplementedException("Read only");
        }

        public T Get<T>() where T : class
        {
            return _entity.Get<T>();
        }

        public virtual IResult Refresh()
        {
            return _entity.Refresh();
        }

        IResult IEntity.Set<T>(T value)
        {
            throw new NotImplementedException("Read only");
        }

        IResult IEntity.Patch(object data)
        {
            throw new NotImplementedException("Read only");
        }

        IResult IEntity.Update(object data)
        {
            throw new NotImplementedException("Read only");
        }

        public void WriteJson(Stream stream)
        {
            _entity.WriteJson(stream);
        }

        public void WriteJson(TextWriter writer)
        {
            _entity.WriteJson(writer);
        }
    }

    internal class Entity<T> : Entity
        where T : class
    {
        public static implicit operator T(Entity<T> entity)
        {
            if (entity == null)
            {
                return null;
            }

            var content = entity._content;

            if (content == null)
            {
                lock (entity)
                {
                    content = entity._content ?? (entity._content = entity.Get<T>());
                }
            }

            return content;
        }

        private T _content;
        private SegmentType _type;

        public Entity(Guid id, T data, Metadata metadata, string name, Segment parent)
            : this(new JsonEntity(id, data, metadata), name, parent)
        {
        }

        public Entity(T data, Metadata metadata, string name, Segment parent)
            : this(Guid.NewGuid(), data, metadata, name, parent)
        {
        }

        public Entity(IEntity entity, string name, Segment parent)
            : base(entity, name, parent)
        {
            _content = null;
        }

        public override SegmentType Type
        {
            get { return _type ?? (_type = (SegmentType)typeof(T).Name); }
        }

        public override IResult Refresh()
        {
            var result = base.Refresh();

            _content = null;

            return result;
        }
    }
}
