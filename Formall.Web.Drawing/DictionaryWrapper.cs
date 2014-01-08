using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Formall
{
    internal class DictionaryWrapper : DynamicObject
    {
        IDictionary<string, object> _dict;

        public DictionaryWrapper(IDictionary<string, object> d)
        {
            _dict = d;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {

            if (_dict.ContainsKey(binder.Name))
            {
                result = _dict[binder.Name];
                return true;
            }

            return base.TryGetMember(binder, out result);
        }
    }
}
