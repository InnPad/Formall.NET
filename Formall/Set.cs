using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall
{
    public class Set<T> : ICollection<T>, IDictionary<string, T>, IEnumerable<T>
        where T : class
    {
        private readonly Func<T, string> _keyGetter;
        private readonly Dictionary<string, T> _dictionary;

        public Set(Func<T, string> keyGetter, Dictionary<string, T> dictionary)
            : base()
        {
            _keyGetter = keyGetter;
            _dictionary = dictionary ?? new Dictionary<string, T>();
        }

        public Set(Func<T, string> keyGetter)
            : base()
        {
            _keyGetter = keyGetter;
            _dictionary = new Dictionary<string, T>();
        }

        #region - ICollection -

        public void Add(T item)
        {
            _dictionary.Add(_keyGetter(item), item);
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public bool Contains(T item)
        {
            return _dictionary.ContainsKey(_keyGetter(item));
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _dictionary.Values.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _dictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return (_dictionary as ICollection<KeyValuePair<string, T>>).IsReadOnly; }
        }

        public bool Remove(T item)
        {
            return _dictionary.Remove(_keyGetter(item));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _dictionary.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion - ICollection -

        #region - IDictionary -

        void IDictionary<string, T>.Add(string key, T value)
        {
            _dictionary.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return _dictionary.Keys; }
        }

        public bool Remove(string key)
        {
            return _dictionary.Remove(key);
        }

        public bool TryGetValue(string key, out T value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        ICollection<T> IDictionary<string, T>.Values
        {
            get { return _dictionary.Values; }
        }

        public T this[string key]
        {
            get
            {
                T value;
                _dictionary.TryGetValue(key, out value);
                return value;
            }
            set { _dictionary[key] = value; }
        }

        void ICollection<KeyValuePair<string, T>>.Add(KeyValuePair<string, T> item)
        {
            (_dictionary as ICollection<KeyValuePair<string, T>>).Add(item);
        }

        void ICollection<KeyValuePair<string, T>>.Clear()
        {
            (_dictionary as ICollection<KeyValuePair<string, T>>).Clear();
        }

        bool ICollection<KeyValuePair<string, T>>.Contains(KeyValuePair<string, T> item)
        {
            return (_dictionary as ICollection<KeyValuePair<string, T>>).Contains(item);
        }

        void ICollection<KeyValuePair<string, T>>.CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
        {
            (_dictionary as ICollection<KeyValuePair<string, T>>).CopyTo(array, arrayIndex);
        }

        int ICollection<KeyValuePair<string, T>>.Count
        {
            get { return (_dictionary as ICollection<KeyValuePair<string, T>>).Count; }
        }

        bool ICollection<KeyValuePair<string, T>>.IsReadOnly
        {
            get { return (_dictionary as ICollection<KeyValuePair<string, T>>).IsReadOnly; }
        }

        bool ICollection<KeyValuePair<string, T>>.Remove(KeyValuePair<string, T> item)
        {
            return (_dictionary as ICollection<KeyValuePair<string, T>>).Remove(item);
        }

        IEnumerator<KeyValuePair<string, T>> IEnumerable<KeyValuePair<string, T>>.GetEnumerator()
        {
            return (_dictionary as IEnumerable<KeyValuePair<string, T>>).GetEnumerator();
        }

        #endregion - IDictionary -
    }
}
