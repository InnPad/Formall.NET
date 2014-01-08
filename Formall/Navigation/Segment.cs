using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Navigation
{
    internal class Segment : ISegment
    {
        private Segment _parent;
        private Set<Segment> _children;

        public Segment(string name, Segment parent)
        {
            Name = name;
            _parent = parent;
        }

        public virtual SegmentClass Class
        {
            get { return SegmentClass.Segment; }
        }

        public string Name
        {
            get;
            set;
        }

        public Segment Parent
        {
            get { return _parent; }
            internal set { _parent = value; }
        }

        protected string Path
        {
            get
            {
                var parent = Parent;
                var name = Name;
                var path = parent != null ? parent.Path : null;

                if (path == null)
                {
                    path = name;
                }
                else if (name != null)
                {
                    path = path + '/' + name;
                }

                return path;
            }
        }

        string ISegment.Name
        {
            get { return this.Name; }
        }

        string ISegment.Path
        {
            get { return this.Path; }
        }

        ISegment ISegment.Parent
        {
            get { return this.Parent; }
        }

        public Set<Segment> Children
        {
            get { return _children ?? (_children = new Set<Segment>((segment) => { return segment.Name; })); }
        }

        IDictionary<string, ISegment> ISegment.Children
        {
            get { return this.Children.AsEnumerable<KeyValuePair<string, Segment>>().ToDictionary(o => o.Key, o => (ISegment)o.Value); }
        }

        public virtual SegmentType Type
        {
            get { return SegmentType.Namespace; }
        }
    }

    internal class Set<T> : ICollection<T>, IDictionary<string, T>
        where T : class
    {
        private readonly Func<T, string> _keyGetter;
        private readonly Dictionary<string, T> _dictionary;

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
