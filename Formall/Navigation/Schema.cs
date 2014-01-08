using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Formall.Navigation
{
    using Formall.Presentation;
    using Formall.Persistence;
    using Formall.Reflection;
    using System.Globalization;

    public class Schema
    {
        private readonly ConcurrentDictionary<string, Entity<Domain>> _container;

        private Schema()
        {
            _container = new ConcurrentDictionary<string, Entity<Domain>>();
        }

        private ISegment ToSegment(IDocument document, Segment root)
        {
            ISegment segment = null;

            if (document != null)
            {
                var entity = document as IEntity;

                string name = null;

                if (entity != null)
                {
                    name = entity.Data.Name as string;
                }

                if (name == null)
                {
                    var file = document as IFileSystem;

                    if (file != null)
                    {
                        name = file.Name;
                    }
                }

                var path = new Queue<string>(name.Split('/'));

                segment = root.Select(path);

                if (path.Count != 0)
                {
                    segment = root.Insert(document);
                }
            }

            return segment;
        }

        public IEnumerable<IDocument> Get(string keyPrefix, Guid id, string host)
        {
            var options = RouteOption.FromHost(host);

            foreach (var current in options)
            {
                Entity<Domain> root;

                if (_container.TryGetValue(current.Pattern, out root))
                {
                    var domain = (Domain)root;

                    var context = root.Context;

                    if (context != null)
                    {
                        var document = context.Read(keyPrefix + id);

                        if (document != null)
                        {
                            yield return document;
                        }
                    }
                }
            }

            yield break;
        }

        public IEnumerable<IDocument> Get(string keyPrefix, string host)
        {
            var options = RouteOption.FromHost(host);

            foreach (var current in options)
            {
                Entity<Domain> root;

                if (_container.TryGetValue(current.Pattern, out root))
                {
                    var domain = (Domain)root;

                    var context = root.Context;

                    if (context != null)
                    {
                        const int pageSize = 65536;
                        int count;
                        for (count = 0; ; )
                        {
                            var page = context.Read(keyPrefix, count, pageSize);

                            for (var i = 0; i < page.Length; i++)
                            {
                                yield return page[i];
                            }

                            count += page.Length;

                            if (page.Length < pageSize)
                                break;
                        }
                    }
                }
            }

            yield break;
        }

        public IEnumerable<ISegment> Query(string name, string host)
        {
            name = name.TrimStart('/');

            var options = RouteOption.FromHost(host);

            foreach (var current in options)
            {
                Entity<Domain> entity;

                if (_container.TryGetValue(current.Pattern, out entity))
                {
                    var domain = (Domain)entity;

                    var path = new Queue<string>(name.Split('/'));

                    var segment = entity.Select(path);

                    if (segment != null && path.Count == 0)
                    {
                        yield return segment;
                    }
                }
            }

            yield break;
        }

        public ISegment Load(Guid id, string host, Domain domain, IDocumentContext context)
        {
            var entity = new Entity<Domain>(domain, new Metadata { Key = "Domain/" + id }, string.Empty, null)
            {
                Context = context as IDataContext
            };

            _container.AddOrUpdate(host, entity, (key, previous) => { return entity; });

            Load(context, entity);

            return entity;
        }

        internal void Load(IDocumentContext context, Segment root)
        {
            const int pageSize = 65536;
            int count;
            for (count = 0; ; )
            {
                var page = context.Read(count, pageSize);

                for (var i = 0; i < page.Length; i++)
                {
                    root.Insert(page[i]);
                }

                count += page.Length;

                if (page.Length < pageSize)
                    break;
            }
        }

        public RouteOption Match(string host)
        {
            RouteOption match = null;

            var options = RouteOption.FromHost(host);

            foreach (var current in options)
            {
                Entity<Domain> entity;
                if (_container.TryGetValue(current.Pattern, out entity))
                {
                    match = current;
                    break;
                }
            }

            return match;
        }

        private static Schema _current;

        private readonly static object _lock = new object();

        public static Schema Current
        {
            get
            {
                var current = _current;

                if (current == null)
                {
                    // lock and check again
                    lock (_lock)
                    {
                        // create new instance if doesn't exist
                        current = _current ?? (_current = new Schema());
                    }
                }

                return current;
            }
        }
    }

    public static class SchemaExtensions
    {
        public static ISegment ParentOf(this Schema domain, ISegment segment)
        {
            return null;
        }
    }
}
