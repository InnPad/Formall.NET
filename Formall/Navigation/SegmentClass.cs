using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Navigation
{
    public class SegmentClass
    {
        readonly string _value;

        private SegmentClass(string value)
        {
            _value = value;
        }

        public static readonly SegmentClass Document = new SegmentClass("Document");

        public static readonly SegmentClass Entity = new SegmentClass("Entity");

        public static readonly SegmentClass Segment = new SegmentClass("Segment");

        public static implicit operator string(SegmentClass sc)
        {
            return sc != null ? sc._value : null;
        }

        public static implicit operator SegmentClass(string value)
        {
            SegmentClass result;

            value = value.ToLowerInvariant();

            if (_dictionary.TryGetValue(value, out result))
            {
                return result;
            }

            result = typeof(SegmentClass).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                .Where(field => string.Equals(field.Name, value, StringComparison.OrdinalIgnoreCase))
                .Select(field => field.GetValue(null))
                .OfType<SegmentClass>()
                .FirstOrDefault();

            return result ??  _dictionary.AddOrUpdate(value, new SegmentClass(value), (key, existing) => { return existing; });
        }

        private static readonly ConcurrentDictionary<string, SegmentClass> _dictionary = new ConcurrentDictionary<string, SegmentClass>(
            new Dictionary<string, SegmentClass>
            {
                { Document, Document },
                { Entity, Entity },
                { Segment, Segment }
            });
    }
}
