using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Formall.Persistence
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;

    internal class ZipIndex
    {
        public const string IndexEntryName = "_index.json";

        private readonly ZipDocumentContext _context;
        private readonly ReaderWriterLockSlim _lock;
        private Set<ZipEntry> _byKey;
        private Set<ZipEntry> _byName;

        public ZipIndex(ZipDocumentContext context)
        {
            _context = context;
            _lock = new ReaderWriterLockSlim();
            _byKey = new Set<ZipEntry>((item) => { return item.Metadata.Key; });
            _byName = new Set<ZipEntry>((item) => { return item.Name; });
        }

        public ZipEntry this[string key]
        {
            get
            {
                ZipEntry entry;

                _lock.EnterReadLock();

                entry = _byKey[key];

                _lock.ExitReadLock();

                return entry;
            }
        }

        public void Delete(ZipEntry entry)
        {
            if (entry != null)
            {
                _lock.EnterWriteLock();

                _byName.Remove(entry.Name);
                _byKey.Remove(entry.Metadata.Key);

                _lock.ExitWriteLock();
            }
        }

        public ZipEntry Get(string name)
        {
            ZipEntry entry;

            _lock.EnterReadLock();

            entry = _byName[name];

            _lock.ExitReadLock();

            return entry;
        }

        public void Load()
        {
            var archive = (ZipArchive)_context;
            var entry = archive.GetEntry(IndexEntryName);

            if (entry != null)
            {
                JObject data;

                using (var reader = new JsonTextReader(new StreamReader(entry.Open())))
                {
                    data = JObject.Load(reader);
                }

                var serializer = new JsonSerializer
                {
                    DateParseHandling = DateParseHandling.None,
                    ContractResolver = new DefaultContractResolver()
                };

                var index = (Dictionary<string, Metadata>)serializer.Deserialize(new JTokenReader(data), typeof(Dictionary<string, Metadata>));

                var byKey = new Set<ZipEntry>((item) => { return item.Metadata.Key; });
                var byName = new Set<ZipEntry>((item) => { return item.Name; });

                foreach (var pair in index)
                {
                    if (pair.Key == null)
                    {
                        continue;
                    }

                    if (pair.Value == null)
                    {
                        continue;
                    }

                    if (pair.Value.Key == null)
                    {
                        continue;
                    }

                    var e = new ZipEntry { Name = pair.Key, Metadata = pair.Value };

                    byKey[pair.Value.Key] = e;
                    byName[pair.Key] = e;
                }

                _lock.EnterWriteLock();

                _byKey = byKey;
                _byName = byName;

                _lock.ExitWriteLock();
            }
        }

        public void Set(string name, Metadata metadata)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (name == string.Empty)
            {
                throw new ArgumentException("name");
            }

            if (metadata == null)
            {
                throw new ArgumentNullException("metadata");
            }

            if (string.IsNullOrEmpty(metadata.Key))
            {
                throw new ArgumentException("metadata");
            }

            ZipEntry entry;

            _lock.EnterReadLock();

            if (_byName.TryGetValue(name, out entry))
            {
                entry.Metadata = metadata;
            }
            else if (_byKey.TryGetValue(metadata.Key, out entry))
            {
                entry.Metadata = metadata;
            }
            else
            {
                entry = new ZipEntry { Name = name, Metadata = metadata };
            }

            _lock.ExitReadLock();

            _lock.EnterWriteLock();

            _byName[name] = entry;
            _byKey[metadata.Key] = entry;

            _lock.ExitWriteLock();
        }

        public void Unset(string name)
        {
            _lock.EnterReadLock();

            var entry = _byName[name];

            _lock.ExitReadLock();

            if (entry != null)
            {
                _lock.EnterWriteLock();

                _byName.Remove(name);
                var key = entry.Metadata.Key;
                _byKey.Remove(key);

                _lock.ExitWriteLock();
            }
        }

        public void Save()
        {
            var entry = _context.Archive.GetEntry(IndexEntryName);

            if (entry == null)
            {
                entry = _context.Archive.CreateEntry(IndexEntryName, _context.CompressionLevel);
            }

            var serializer = new JsonSerializer
            {
                DateParseHandling = DateParseHandling.None,
                ContractResolver = new DefaultContractResolver()
            };

            _lock.EnterReadLock();

            var dictionary = _byName.AsEnumerable<ZipEntry>().ToDictionary(o => o.Name, o => o.Metadata);

            _lock.ExitReadLock();

            var data = JObject.FromObject(dictionary, serializer);

            using (var writer = new StreamWriter(entry.Open()))
            {
                writer.Write(new JTokenReader(data));
            }
        }
    }
}
