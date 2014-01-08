using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Formall.Persistence
{
    public class FileIndex : IEnumerable<FileEntry>
    {
        public const string IndexEntryName = "_index.json";

        private readonly FileDocumentContext _context;
        private readonly ReaderWriterLockSlim _lock;
        private Set<FileEntry> _byKey;
        private Set<FileEntry> _byName;

        public FileIndex(FileDocumentContext context)
        {
            _context = context;
            _lock = new ReaderWriterLockSlim();
            _byKey = new Set<FileEntry>((item) => { return item.Metadata.Key; });
            _byName = new Set<FileEntry>((item) => { return item.Name; });
        }

        public FileEntry this[string key]
        {
            get
            {
                FileEntry entry;

                _lock.EnterReadLock();

                entry = _byKey[key];

                _lock.ExitReadLock();

                return entry;
            }
        }

        public string[] Keys
        {
            get { return _byKey.Keys.ToArray(); }
        }

        public void Delete(FileEntry entry)
        {
            if (entry != null)
            {
                _lock.EnterWriteLock();

                _byName.Remove(entry.Name);
                _byKey.Remove(entry.Metadata.Key);

                _lock.ExitWriteLock();
            }
        }

        public FileEntry Get(string name)
        {
            FileEntry entry;

            _lock.EnterReadLock();

            entry = _byName[name];

            _lock.ExitReadLock();

            return entry;
        }

        public void Load()
        {
            var directory = (DirectoryInfo)_context;

            var file = new FileInfo(Path.Combine(directory.FullName, IndexEntryName));

            if (file.Exists)
            {
                JObject data;

                using (var reader = new JsonTextReader(new StreamReader(file.Open(FileMode.Open))))
                {
                    try
                    {
                        data = JObject.Load(reader);
                    }
                    catch
                    {
                        data = new JObject();
                    }
                }

                var serializer = new JsonSerializer
                {
                    DateParseHandling = DateParseHandling.None,
                    ContractResolver = new DefaultContractResolver()
                };

                var index = (Dictionary<string, FileMetadata>)serializer.Deserialize(new JTokenReader(data), typeof(Dictionary<string, FileMetadata>));

                var byKey = new Set<FileEntry>((item) => { return item.Metadata.Key; });
                var byName = new Set<FileEntry>((item) => { return item.Name; });

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

                    var e = new FileEntry { Name = pair.Key, Metadata = pair.Value };

                    byKey[pair.Value.Key] = e;
                    byName[pair.Key] = e;
                }

                _lock.EnterWriteLock();

                _byKey = byKey;
                _byName = byName;

                _lock.ExitWriteLock();
            }
        }

        public void Set(string name, FileMetadata metadata)
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

            FileEntry entry;

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
                entry = new FileEntry { Name = name, Metadata = metadata };
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
            var directory = (DirectoryInfo)_context;
            var file = new FileInfo(Path.Combine(directory.FullName, IndexEntryName));

            var serializer = new JsonSerializer
            {
                DateParseHandling = DateParseHandling.None,
                ContractResolver = new DefaultContractResolver()
            };

            _lock.EnterReadLock();

            var dictionary = _byName.AsEnumerable<FileEntry>().ToDictionary(o => o.Name, o => o.Metadata);

            _lock.ExitReadLock();

            var data = JObject.FromObject(dictionary, serializer);

            using (var stream = file.Open(FileMode.OpenOrCreate | FileMode.Truncate))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(new JTokenReader(data));
                }
            }
        }

        public IEnumerator<FileEntry> GetEnumerator()
        {
            return _byName.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
