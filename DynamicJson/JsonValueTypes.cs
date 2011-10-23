using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicJson
{

    public abstract class JsonValueType
    {
        protected JsonValueType() { }

        public abstract bool ValueQualifies(object value);
        public abstract JsonValue GetValue(object value);

        public static bool operator ==(JsonValueType a, JsonValueType b)
        {
            return System.Object.ReferenceEquals(a, b);
        }

        public static bool operator !=(JsonValueType a, JsonValueType b)
        {
            return !System.Object.ReferenceEquals(a, b);
        }

        public override bool Equals(object obj)
        {
            return System.Object.ReferenceEquals(this, obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        
    }

    public class JsonBooleanType : JsonValueType
    {
        private JsonBooleanType() : base() { }

        public static readonly JsonBooleanType INSTANCE = new JsonBooleanType();

        public override bool ValueQualifies(object value)
        {
            return value != null && value.GetType().Equals(typeof(bool));
        }

        public override JsonValue GetValue(object value)
        {
            return new JsonBoolean((bool)value);
        }
    }

    public class JsonNullType : JsonValueType
    {
        private JsonNullType() : base() { }

        public static readonly JsonNullType INSTANCE = new JsonNullType();

        public override bool ValueQualifies(object value)
        {
            return null == value;
        }

        public override JsonValue GetValue(object value)
        {
            return new JsonNull();
        }
    }

    public class JsonStringType : JsonValueType
    {
        private JsonStringType() : base() { }

        public static readonly JsonStringType INSTANCE = new JsonStringType();

        public override bool ValueQualifies(object value)
        {
            return value != null && value.GetType().Equals(typeof(string));
        }

        public override JsonValue GetValue(object value)
        {
            return new JsonString((string)value);
        }
    }

    public class JsonNumberType : JsonValueType
    {
        private readonly static Tuple<Type, Func<object, JsonValue>>[] ACCEPTABLE_NUMERIC_TYPES = new Tuple<Type, Func<object, JsonValue>>[]
                {
                    new Tuple<Type, Func<object, JsonValue>>(typeof(int), o => new JsonNumber((int) o)),
                    new Tuple<Type, Func<object, JsonValue>>(typeof(uint), o => new JsonNumber((uint) o)),
                    new Tuple<Type, Func<object, JsonValue>>(typeof(byte), o => new JsonNumber((byte) o)),
                    new Tuple<Type, Func<object, JsonValue>>(typeof(sbyte), o => new JsonNumber((sbyte) o)),
                    new Tuple<Type, Func<object, JsonValue>>(typeof(long), o => new JsonNumber((long) o)),
                    new Tuple<Type, Func<object, JsonValue>>(typeof(ulong), o => new JsonNumber((ulong) o)),
                    new Tuple<Type, Func<object, JsonValue>>(typeof(short), o => new JsonNumber((short) o)),
                    new Tuple<Type, Func<object, JsonValue>>(typeof(ushort), o => new JsonNumber((ushort) o)),
                    new Tuple<Type, Func<object, JsonValue>>(typeof(double), o => new JsonNumber((double) o)),
                    new Tuple<Type, Func<object, JsonValue>>(typeof(float), o => new JsonNumber((float) o)),
                    new Tuple<Type, Func<object, JsonValue>>(typeof(decimal), o => new JsonNumber((decimal) o))
                };

        private JsonNumberType() : base() { }

        public static readonly JsonNumberType INSTANCE = new JsonNumberType();

        public override bool ValueQualifies(object value)
        {
            return value != null && ACCEPTABLE_NUMERIC_TYPES.Any(t => t.Item1.Equals(value.GetType()));
        }

        public override JsonValue GetValue(object value)
        {
            return ACCEPTABLE_NUMERIC_TYPES.First(t => t.Item1.Equals(value.GetType())).Item2(value);
        }
    }

    public class JsonArrayType : JsonValueType
    {
        private JsonArrayType() : base() { }

        public static readonly JsonArrayType INSTANCE = new JsonArrayType();

        public override bool ValueQualifies(object value)
        {
            return value != null && value is IEnumerable<object>;
        }

        public override JsonValue GetValue(object value)
        {
            return new JsonArray(JsonValueTypes.Interpret((object[]) value));
        }
    }

    public class JsonObjectType : JsonValueType
    {
        private JsonObjectType() : base() { }

        public static readonly JsonObjectType INSTANCE = new JsonObjectType();

        public override bool ValueQualifies(object value)
        {
            var other = value as System.Collections.IEnumerable;
            if (other == null) return false;

            bool isKVPs = true;
            foreach (var item in other)
            {
                isKVPs = item.GetType().GetProperty("Key") != null && item.GetType().GetProperty("Value") != null;
            }
            return isKVPs;
        }

        public override JsonValue GetValue(object value)
        {
            var other = (System.Collections.IEnumerable)value;
            var listOfKvps = new List<KeyValuePair<string, object>>();
            foreach (var item in other)
            {
                var key = item.GetType().GetProperty("Key").GetValue(item, null).ToString();
                var val = item.GetType().GetProperty("Value").GetValue(item, null);

                listOfKvps.Add(new KeyValuePair<string, object>(key, val));
            }
            return new JsonObject(JsonValueTypes.Interpret(listOfKvps));
        }
    }

    public static class JsonValueTypes
    {
        public static readonly JsonValueType OBJECT = JsonObjectType.INSTANCE;
        public static readonly JsonValueType ARRAY = JsonArrayType.INSTANCE;
        public static readonly JsonValueType STRING = JsonStringType.INSTANCE;
        public static readonly JsonValueType NUMBER = JsonNumberType.INSTANCE;
        public static readonly JsonValueType BOOL = JsonBooleanType.INSTANCE;
        public static readonly JsonValueType NULL = JsonNullType.INSTANCE;
        
        public static readonly JsonValueType[] ALL_TYPE = new[]
            {
                OBJECT,
                ARRAY,
                STRING,
                NUMBER,
                BOOL,
                NULL
            };


        public static IEnumerable<KeyValuePair<string, JsonValue>> Interpret(IEnumerable<KeyValuePair<string, object>> pairs)
        {
            return pairs
                .Select(kvp => new KeyValuePair<string, JsonValue>(kvp.Key, Interpret(kvp.Value)));
        }

        public static JsonValue[] Interpret(object[] objArray)
        {
            return objArray
                .Select(o => Interpret(o))
                .ToArray();
        }

        public static JsonValue Interpret(object o)
        {
            return ALL_TYPE
                .First(t => t.ValueQualifies(o))
                .GetValue(o);
        }
    }

}
