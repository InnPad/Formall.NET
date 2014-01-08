using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Formall.Persistence
{
    using Formall.Navigation;
    using Formall.Reflection;
    using Raven.Abstractions.Data;
    using Raven.Abstractions.Linq;
    using Raven.Client;
    using Raven.Client.Document;
    using Raven.Client.Embedded;
    using Raven.Client.Indexes;
    using Raven.Imports.Newtonsoft.Json;
    using Raven.Imports.Newtonsoft.Json.Serialization;
    using Raven.Json.Linq;

    public class RavenDocumentContext : IDataContext
    {
        private readonly object _lock = new object();

        private readonly DocumentStoreConfig _config;
        private readonly string _name;
        private DocumentStoreBase _store;
        private readonly Schema _schema;
        private readonly string _host;
        private readonly ConcurrentDictionary<string, Repository> _repositories;

        private RavenDocumentContext(Schema schema, string host)
        {
            _schema = schema;
            _host = host;
            _repositories = new ConcurrentDictionary<string, Repository>();
        }

        public RavenDocumentContext(string name, string host, Schema schema)
            : this(schema, host)
        {
            _name = name;

            var segment = schema.Query(name, host);

            if (segment == null)
            {
                // log error
            }
            else
            {
                var entity = segment as IEntity;

                if (entity == null)
                {
                    // log error
                }
                else
                {
                    _config = entity.Get<DocumentStoreConfig>();
                }
            }
        }

        public RavenDocumentContext(string host, Schema schema, object config)
            : this(schema, host)
        {
            var serializer = new JsonSerializer
            {
                DateParseHandling = DateParseHandling.None,
                ContractResolver = new DefaultContractResolver()
            };

            _config = (DocumentStoreConfig)serializer.Deserialize(new RavenJTokenReader(RavenJObject.FromObject(config)), typeof(DocumentStoreConfig));
            _name = _config.Name;
        }

        internal protected IDocumentStore DocumentStore
        {
            get
            {
                var store = _store;

                if (store == null)
                {
                    //
                    // lock and check again

                    lock (_lock)
                    {
                        store = _store;

                        if (store == null)
                        {
                            if (_config == null)
                            {
                                // error already logged
                            }
                            else try
                                {
                                    //
                                    // create new instance if doesn't exist

                                    var apiKey = _config.ApiKey + '/' + _config.Secret;

                                    if (_config.Embeddable)
                                    {
                                        store = new EmbeddableDocumentStore
                                        {
                                            //ApiKey = apiKey,
                                            //ConnectionStringName = _config.Name,
                                            DataDirectory = _config.DataDirectory
                                        };
                                    }
                                    else
                                    {
                                        store = new DocumentStore
                                        {
                                            //ApiKey = apiKey,
                                            //ConnectionStringName = _config.Name,
                                            Url = _config.Url
                                        };
                                    }

                                    if (store != null)
                                    {
                                        //
                                        // define store conventions

                                        //store.Conventions.IdentityPartsSeparator = "/";
                                        store.Conventions.SaveEnumsAsIntegers = false;

                                        //
                                        // initialize store instance

                                        store.Initialize();

                                        //
                                        // create index by entity name

                                        new RavenDocumentsByEntityName().Execute(store);
                                    }
                                }
                                catch (Exception)
                                {
                                    // log exception
                                }
                                finally
                                {
                                    // save store instance
                                    _store = store;
                                }
                        }
                    }
                }

                return store;
            }
        }

        IRepository IDataContext.CreateRepository(string name)
        {
            return this.Repository(name);
        }

        internal Entity Import(RavenJObject data, Metadata metadata)
        {
            Guid id;
            var key = metadata.Key;
            var type = key.Exclude(1, '/');

            Guid.TryParse(key.Split('/').Last(), out id);

            var entity = new Entity(id, data, metadata.FromMetadata(), Repository(type));

            return Store(entity);
        }

        internal Entity Import(IDocument document)
        {
            RavenJObject data;

            var serializer = new JsonSerializer
            {
                DateParseHandling = DateParseHandling.None,
                ContractResolver = new DefaultContractResolver()
            };

            using (var reader = new StreamReader(document.Content))
            {
                data = RavenJObject.Load(new JsonTextReader(reader));
            }

            return Import(data, document.Metadata);
        }

        IEntity IDataContext.Import(IEntity entity)
        {
            return Store(Import(entity));
        }

        private Entity Store(Entity entity)
        {
            var result = DocumentStore.DatabaseCommands.Put(entity.Key, null, entity.Data, entity.Metadata);

            /*return new SuccessResult
            {
                Data = new { Key = result.Key, Etag = result.ETag }
            };*/

            return entity;
        }

        #region - IDocumentContext -

        public IResult Delete(string key)
        {
            DocumentStore.DatabaseCommands.Delete(key, null);
            return null;
        }

        public IDocument Import(Stream stream, MediaType type, Metadata metadata)
        {
            IDocument document;

            using (var reader = new StreamReader(stream))
            {
                document = Import(reader, type, metadata);
            }

            return document;
        }

        public IDocument Import(TextReader reader, MediaType type, Metadata metadata)
        {
            RavenJObject data;

            if (type == MediaType.Json)
            {
                data = RavenJObject.Load(new JsonTextReader(reader));
            }
            else
            {
                // TODO: support import of other content types like XML
                throw new NotImplementedException("RavenDocumentContext doesnt support importing other content type than JSON.");
            }

            return Import(data, metadata);
        }

        IDocument IDocumentContext.Import(IDocument document)
        {
            return Import(document);
        }

        public IDocument[] Read(int skip, int take)
        {
            var documents = DocumentStore.DatabaseCommands.GetDocuments(skip, take);

            return documents.Select(o => new Entity(Entity.ParseId(o.Key), o.DataAsJson, o.Metadata, Repository(o.Metadata.Value<string>("Raven-Entity-Name")))).OfType<IDocument>().ToArray();
        }

        public IDocument Read(string key)
        {
            Entity entity = null;

            var document = DocumentStore.DatabaseCommands.Get(key);

            if (document != null)
            {
                var data = document.DataAsJson;
                var metadata = document.Metadata;
                entity = new Entity(Entity.ParseId(key), data, metadata, Repository(metadata.Value<string>("Raven-Entity-Name")));
            }

            return entity;
        }

        public IDocument[] Read(string keyPrefix, int skip, int take)
        {
            var documents = DocumentStore.DatabaseCommands.StartsWith(keyPrefix, null, skip, take);

            return documents.Select(o => new Entity(Entity.ParseId(o.Key), o.DataAsJson, o.Metadata, Repository(o.Metadata.Value<string>("Raven-Entity-Name")))).OfType<IDocument>().ToArray();
        }

        internal Repository Repository(string name)
        {
            var keyPrefix = name + '/';

            Repository repository;

            if (!_repositories.TryGetValue(name, out repository))
            {
                var segment = _schema.Query(name, _host).FirstOrDefault();

                //string keyPrefix = null;
                Model model = null;

                if (segment != null)
                {
                    var entity = segment as Entity<Model>;
                    if (entity != null)
                    {
                        model = (Model)entity;

                        //keyPrefix = model.Name + '/';

                        /*for (var baseType = model.BaseType; baseType != null; )
                        {
                            segment = segment.Parent;
                            if (segment != null)
                            {
                                entity = segment as Entity<Model>;
                                var baseModel = (Model)entity;
                                //keyPrefix = baseModel.Name + '/' + keyPrefix;
                            }

                        }*/
                    }
                }

                repository = _repositories.AddOrUpdate(name, new Repository(keyPrefix, model, this), (key, previous) =>
                {
                    // log info to track worst cases
                    return previous;
                });
            }

            return repository;
        }

        #endregion - IDocumentContext -
    }
}
