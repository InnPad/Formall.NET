using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Persistence
{
    using Formall.Reflection;

    public class DocumentContextMapper
    {
        class Entry
        {
            private IDocument _config;
            private IDocumentContext _instance;
            private readonly string _name;
            private readonly Prototype _type;
        }

        private readonly static object _lock = new object();

        private static DocumentContextMapper _current;

        public static DocumentContextMapper Current
        {
            get
            {
                var current = _current;

                if (current == null)
                {
                    lock (_lock)
                    {
                        current = _current ?? (_current = new DocumentContextMapper());
                    }
                }

                return current;
            }
        }

        private readonly ConcurrentDictionary<string, Entry> _mapping;

        private DocumentContextMapper()
        {
            _mapping = new ConcurrentDictionary<string, Entry>();
        }

        public void Map<TDocumentContext>(string name, IDocument config)
            where TDocumentContext : class, IDocumentContext
        {
        }

        public IDocumentContext Get(string name)
        {
            throw new NotImplementedException();
        }
    }
}
