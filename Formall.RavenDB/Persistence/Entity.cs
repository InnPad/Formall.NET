using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Formall.Persistence
{
    using Formall.Reflection;
    using Raven.Abstractions.Linq;
    using Raven.Imports.Newtonsoft.Json;
    using Raven.Imports.Newtonsoft.Json.Serialization;
    using Raven.Json.Linq;
    

    internal class Entity : IDocument, IEntity
    {
        public static Guid ParseId(string key)
        {
            Guid id;
            Guid.TryParse(key.Split('/').Last(), out id);
            return id;
        }

        private readonly Guid _id;
        private readonly string _key;
        private readonly Repository _repository;
        private RavenJObject _data;
        private RavenJObject _metadata;

        public Entity(Guid id, RavenJObject data, RavenJObject metadata, Repository repository)
        {
            _id = id;
            _key = repository.KeyPrefix + id;
            _data = data;
            _metadata = metadata;
            _repository = repository;
        }

        public RavenJObject Data
        {
            get { return _data; }
            set { _data = value; }
        }

        dynamic IEntity.Data
        {
            get { return new DynamicJsonObject(_data ?? (_data = new RavenJObject())); }
        }

        public Guid Id
        {
            get { return _id; }
        }

        public RavenJObject Metadata
        {
            get { return _metadata; }
        }

        public Model Model
        {
            get { return _repository.Model; }
        }

        public IRepository Repository
        {
            get { return _repository; }
        }

        public IResult Delete()
        {
            return _repository.Delete(Id);
        }

        public T Get<T>(JsonConverter[] converters) where T : class
        {
            var jsonSerializer = new JsonSerializer
            {
                DateParseHandling = DateParseHandling.None,
                ContractResolver = /*new CamelCasePropertyNamesContractResolver()*/ new DefaultContractResolver()
            };

            if (converters != null)
            {
                foreach (var converter in converters)
                {
                    jsonSerializer.Converters.Add(converter);
                }
            }

            return (T)jsonSerializer.Deserialize(new RavenJTokenReader(Data), typeof(T));
        }

        public T Get<T>() where T : class
        {
            return Get<T>(new JsonConverter[0]);
        }

        public IResult Refresh()
        {
            _repository.Read(Id);
            return null;
        }

        public IResult Set<T>(T value) where T : class
        {
            _data = RavenJObject.FromObject(value);

            return null;
        }

        public IResult Patch(object data)
        {
            throw new NotImplementedException();
        }

        public IResult Update(object data)
        {
            throw new NotImplementedException();
        }

        public void WriteJson(Stream stream)
        {
            using (var writer = new StreamWriter(stream))
            {
                WriteJson(writer);
            }
        }

        public void WriteJson(TextWriter writer)
        {
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                jsonWriter.Formatting = Formatting.Indented;
                Data.WriteTo(jsonWriter);
            }
        }

        #region - IDocument -

        Stream IDocument.Content
        {
            get
            {
                var stream = new MemoryStream();

                var serializer = new JsonSerializer
                {
                };

                var writer = new StreamWriter(stream, ContentEncoding);

                writer.Write(Data.ToString());

                //serializer.Serialize(new JsonTextWriter(new StreamWriter(stream, ContentEncoding)) { Formatting = Formatting.Indented }, Data);


                stream.Seek(0L, SeekOrigin.Begin);

                return stream;
            }
        }

        public Encoding ContentEncoding
        {
            get { return Encoding.UTF8; }
        }

        IDocumentContext IDocument.Context
        {
            get { return _repository.Context; }
        }

        public string Key
        {
            get { return _key; }
        }

        MediaType IDocument.ContentType
        {
            get { return MediaType.Json; }
        }

        Metadata IDocument.Metadata
        {
            get { return _metadata.ToMetadata(); }
        }

        #endregion - IDocument -
    }
}
