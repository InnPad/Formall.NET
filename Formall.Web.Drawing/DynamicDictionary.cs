using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Formall
{
    public class DynamicDictionary : DynamicObject, IDictionary<string, object>
    {
        IDictionary<string, object> d;

        public DynamicDictionary()
            : this(new Dictionary<string, object>())
        { }

        public DynamicDictionary(IDictionary<string, object> viewdata)
        {
            this.d = viewdata;
        }

        #region "DynamicObject Overrides"

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (this.d.ContainsKey(binder.Name))
            {
                result = this.d[binder.Name];
                return true;
            }
            return base.TryGetMember(binder, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this.d[binder.Name] = value;
            return true;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return this.d.Keys;
        }

        public override bool TryDeleteMember(DeleteMemberBinder binder)
        {
            if (this.d.ContainsKey(binder.Name))
            {
                this.d.Remove(binder.Name);
            }
            return true;
        }

        #endregion

        #region "IDictionary Methods"

        void IDictionary<string, object>.Add(string key, object value)
        {
            this.d.Add(key, value);
        }

        bool IDictionary<string, object>.ContainsKey(string key)
        {
            return this.d.ContainsKey(key);
        }

        ICollection<string> IDictionary<string, object>.Keys
        {
            get { return this.d.Keys; }
        }

        bool IDictionary<string, object>.Remove(string key)
        {
            return this.d.Remove(key);
        }

        bool IDictionary<string, object>.TryGetValue(string key, out object value)
        {
            return this.d.TryGetValue(key, out value);
        }

        ICollection<object> IDictionary<string, object>.Values
        {
            get { return this.d.Values; }
        }

        object IDictionary<string, object>.this[string key]
        {
            get
            {
                return this.d[key];
            }
            set
            {
                this.d[key] = value;
            }
        }

        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
        {
            this.d.Add(item);
        }

        void ICollection<KeyValuePair<string, object>>.Clear()
        {
            this.d.Clear();
        }

        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
        {
            return this.d.Contains(item);
        }

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            this.d.CopyTo(array, arrayIndex);
        }

        int ICollection<KeyValuePair<string, object>>.Count
        {
            get { return this.d.Count; }
        }

        bool ICollection<KeyValuePair<string, object>>.IsReadOnly
        {
            get { return this.d.IsReadOnly; }
        }

        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
        {
            return this.d.Remove(item);
        }

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            return this.d.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.d.GetEnumerator();
        }

        #endregion
    }
}
