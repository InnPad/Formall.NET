using System;
using System.Dynamic;
using System.IO;
using System.Linq;

namespace Formall.Serialization
{
    using Formall.Persistence;
    using Formall.Presentation;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;
    using System.Text;
    
    public class JsonEntity : IEntity
    {
        public static JsonEntity Clone(IEntity entity)
        {
            return null;
        }

        private readonly string _key;
        private readonly Guid _id;
        private JObject _data;
        private Metadata _metadata;

        public JsonEntity(object data, Metadata metadata)
        {
            _key = metadata.Key;
            Guid.TryParse(_key.Split('/').Last(), out _id);
            _data = JObject.FromObject(data);
            _metadata = metadata;
        }

        public JsonEntity(Guid id, object data, Metadata metadata)
        {
            _id = id;
            _data = JObject.FromObject(data);
            _metadata = metadata;
        }

        dynamic IEntity.Data
        {
            get { return _data ?? (_data = new JObject()); }
        }

        Guid IEntity.Id
        {
            get { return _id; }
        }

        Reflection.Model IEntity.Model
        {
            get { throw new NotImplementedException(); }
        }

        IRepository IEntity.Repository
        {
            get { throw new NotImplementedException(); }
        }

        IResult IEntity.Delete()
        {
            throw new NotImplementedException();
        }

        T IEntity.Get<T>()
        {
            var jsonSerializer = new JsonSerializer
            {
                DateParseHandling = DateParseHandling.None,
                ContractResolver = new DefaultContractResolver()
            };

            return (T)jsonSerializer.Deserialize(new JTokenReader(_data), typeof(T));
        }

        IResult IEntity.Refresh()
        {
            throw new NotImplementedException();
        }

        IResult IEntity.Set<T>(T value)
        {
            _data = JObject.FromObject(value);

            return null;
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

        System.IO.Stream IDocument.Content
        {
            get
            {
                var stream = new MemoryStream();
                var writer = new StreamWriter(stream);

                var serializer = new JsonSerializer
                {
                    DateParseHandling = DateParseHandling.None,
                    ContractResolver = new DefaultContractResolver()
                };

                serializer.Serialize(writer, _data);
                
                writer.Flush();

                stream.Seek(0L, SeekOrigin.Begin);

                return stream;
            }
        }

        public Encoding ContentEncoding
        {
            get { return Encoding.Unicode; }
        }

        IDocumentContext IDocument.Context
        {
            get { return null; }
        }

        MediaType IDocument.ContentType
        {
            get { return MediaType.Json; }
        }

        string IDocument.Key
        {
            get { return _metadata.Key; }
        }

        Metadata IDocument.Metadata
        {
            get { return _metadata; }
        }
    }
}
