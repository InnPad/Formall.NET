using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Navigation
{
    public class SegmentType
    {
        readonly string _value;

        private SegmentType(string value)
        {
            _value = value;
        }

        public static readonly SegmentType Area = new SegmentType("Area");

        public static readonly SegmentType Domain = new SegmentType("Domain");

        public static readonly SegmentType Item = new SegmentType("Item");

        public static readonly SegmentType Model = new SegmentType("Model");

        public static readonly SegmentType Money = new SegmentType("Money");

        public static readonly SegmentType Namespace = new SegmentType("Namespace");

        public static readonly SegmentType Unit = new SegmentType("Unit");

        public static readonly SegmentType Value = new SegmentType("Value");

        public static implicit operator string(SegmentType st)
        {
            return st != null ? st._value : null;
        }

        public static implicit operator SegmentType(string value)
        {
            SegmentType result;

            value = value.ToLowerInvariant();

            if (_dictionary.TryGetValue(value, out result))
            {
                return result;
            }

            result = typeof(SegmentType).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                .Where(field => string.Equals(field.Name, value, StringComparison.OrdinalIgnoreCase))
                .Select(field => field.GetValue(null))
                .OfType<SegmentType>()
                .FirstOrDefault();

            return result ??  _dictionary.AddOrUpdate(value, new SegmentType(value), (key, existing) => { return existing; });
        }

        private static readonly ConcurrentDictionary<string, SegmentType> _dictionary = new ConcurrentDictionary<string, SegmentType>(
            new Dictionary<string, SegmentType>
            {
                { Area, Area },
                { Domain, Domain },
                { Item, Item },
                { Model, Model },
                { Money, Money },
                { Namespace, Namespace },
                { Unit, Unit },
                { Value, Value }
            });
            
    }
}
