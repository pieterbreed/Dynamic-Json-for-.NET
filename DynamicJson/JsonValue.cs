using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Dynamic;
using Antlr.Runtime;
using Antlr.Runtime.Tree;

namespace DynamicJson
{
   [ContractClass(typeof(JsonValueContracts))]
   public abstract class JsonValue : DynamicObject
   {
      protected JsonValue(JsonValueType type)
      {
         Type = type;
      }

      public JsonValueType Type { get; private set; }

      public abstract string MakePrintValue();

      public override string ToString()
      {
         return MakePrintValue();
      }

      public static bool operator ==(JsonValue a, JsonValue b)
      {
         return Object.ReferenceEquals(a, b) || a.Equals(b);
      }

      public static bool operator !=(JsonValue a, JsonValue b)
      {
         return !(a == b);
      }

      #region value type casts
      public static explicit operator string(JsonValue val)
      {
         return (string)(JsonString)val;
      }

      public static explicit operator double(JsonValue val)
      {
         return (double)(JsonNumber)val;
      }

      public static explicit operator byte(JsonValue b)
      {
         return (byte)(JsonNumber)b;
      }

      public static explicit operator sbyte(JsonValue sb)
      {
         return (sbyte)(JsonNumber)sb;
      }

      public static explicit operator decimal(JsonValue d)
      {
         return (decimal)(JsonNumber)d;
      }

      public static explicit operator float(JsonValue f)
      {
         return (float)(JsonNumber)f;
      }

      public static explicit operator int(JsonValue i)
      {
         return (int)(JsonNumber)i;
      }

      public static explicit operator uint(JsonValue ui)
      {
         return (uint)(JsonNumber)ui;
      }

      public static explicit operator long(JsonValue l)
      {
         return (long)(JsonNumber)l;
      }

      public static explicit operator ulong(JsonValue ul)
      {
         return (ulong)(JsonNumber)ul;
      }

      public static explicit operator short(JsonValue s)
      {
         return (short)(JsonNumber)s;
      }

      public static explicit operator ushort(JsonValue us)
      {
         return (ushort)(JsonNumber)us;
      }

      public static explicit operator bool(JsonValue b)
      {
         return (bool)(JsonBoolean)b;
      }
      #endregion

      [Pure]
      public bool IsString { get { return Type == JsonValueTypes.STRING; } }
      [Pure]
      public bool IsBool { get { return Type == JsonValueTypes.BOOL; } }
      [Pure]
      public bool IsNumber { get { return Type == JsonValueTypes.NUMBER; } }
      [Pure]
      public bool IsNull { get { return Type == JsonValueTypes.NULL; } }
      [Pure]
      public bool IsArray { get { return Type == JsonValueTypes.ARRAY; } }
      [Pure]
      public bool IsObject { get { return Type == JsonValueTypes.OBJECT; } }

   }

   [ContractClassFor(typeof(JsonValue))]
   class JsonValueContracts : JsonValue
   {
      public JsonValueContracts(JsonValueType type) : base(type)
      {
      }

      public override string MakePrintValue()
      {
         Contract.Ensures(Contract.Result<string>() != null);
         throw new NotImplementedException();
      }
   }

   public class JsonString : JsonValue
   {
      public JsonString(string s)
         : base(JsonValueTypes.STRING)
      {
         Value = s;
      }

      public string Value { get; private set; }

      public static implicit operator string(JsonString s)
      {
         return s.Value;
      }

      public override string MakePrintValue()
      {
         return MakeStringValue(Value);
      }

      public static string MakeStringValue(string v)
      {
         return string.Format("\"{0}\"", v);
      }

      public override bool Equals(object obj)
      {
         var other = obj as JsonString;
         return other != null && other.Value.Equals(this.Value);
      }

      public override int GetHashCode()
      {
         return this.Value.GetHashCode();
      }

   }

   public class JsonNumber : JsonValue
   {
      public JsonNumber(double d)
         : base(JsonValueTypes.NUMBER)
      {
         Value = d;
      }

      #region other numeric type constructors
      public JsonNumber(int i)
         : base(JsonValueTypes.NUMBER)
      {
         Value = i;
      }

      public JsonNumber(byte b)
         : base(JsonValueTypes.NUMBER)
      {
         Value = b;
      }

      public JsonNumber(sbyte b)
         : base(JsonValueTypes.NUMBER)
      {
         Value = b;
      }

      public JsonNumber(decimal d)
         : base(JsonValueTypes.NUMBER)
      {
         Value = Convert.ToDouble(d);
      }

      public JsonNumber(float f)
         : base(JsonValueTypes.NUMBER)
      {
         Value = f;
      }

      public JsonNumber(uint ui)
         : base(JsonValueTypes.NUMBER)
      {
         Value = ui;
      }

      public JsonNumber(long l)
         : base(JsonValueTypes.NUMBER)
      {
         Value = l;
      }

      public JsonNumber(ulong ul)
         : base(JsonValueTypes.NUMBER)
      {
         Value = ul;
      }

      public JsonNumber(short s)
         : base(JsonValueTypes.NUMBER)
      {
         Value = s;
      }

      public JsonNumber(ushort us)
         : base(JsonValueTypes.NUMBER)
      {
         Value = us;
      }

      #endregion

      public double Value { get; private set; }

      public static implicit operator double(JsonNumber d)
      {
         return d.Value;
      }

      public override string MakePrintValue()
      {
         return string.Format("{0}", Value);
      }

      public override bool Equals(object obj)
      {
         var other = obj as JsonNumber;

         return other != null && other.Value.Equals(this.Value);

      }

      public override int GetHashCode()
      {
         return this.Value.GetHashCode();
      }

      #region other type cast operators

      public static implicit operator byte(JsonNumber b)
      {
         return Convert.ToByte(b.Value);
      }

      public static implicit operator sbyte(JsonNumber sb)
      {
         return Convert.ToSByte(sb.Value);
      }

      public static implicit operator decimal(JsonNumber d)
      {
         return Convert.ToDecimal(d.Value);
      }

      public static implicit operator float(JsonNumber f)
      {
         return Convert.ToSingle(f.Value);
      }

      public static implicit operator int(JsonNumber i)
      {
         return Convert.ToInt32(i.Value);
      }

      public static implicit operator uint(JsonNumber ui)
      {
         return Convert.ToUInt32(ui.Value);
      }

      public static implicit operator long(JsonNumber l)
      {
         return Convert.ToInt64(l.Value);
      }

      public static implicit operator ulong(JsonNumber ul)
      {
         return Convert.ToUInt64(ul.Value);
      }

      public static implicit operator short(JsonNumber s)
      {
         return Convert.ToInt16(s.Value);
      }

      public static implicit operator ushort(JsonNumber us)
      {
         return Convert.ToUInt16(us.Value);
      }

      #endregion

   }

   public class JsonBoolean : JsonValue
   {
      public JsonBoolean(bool b)
         : base(JsonValueTypes.BOOL)
      {
         Value = b;
      }

      public bool Value { get; private set; }

      public static implicit operator bool(JsonBoolean b)
      {
         return b.Value;
      }

      public override string MakePrintValue()
      {
         return Value ? "true" : "false";
      }

      public override bool Equals(object obj)
      {
         var other = obj as JsonBoolean;
         return other != null && other.Value.Equals(this.Value);
      }

      public override int GetHashCode()
      {
         return this.Value.GetHashCode();
      }
   }

   public class JsonNull : JsonValue
   {
      public JsonNull() : base(JsonValueTypes.NULL) { }

      public object Value { get { return null; } }

      public override string MakePrintValue()
      {
         return "null";
      }

      public override int GetHashCode()
      {
         return MakePrintValue().GetHashCode();
      }

      public override bool Equals(object obj)
      {
         return obj is JsonNull;
      }

      public readonly static JsonNull NULL = new JsonNull();

   }

   public class JsonArray : JsonValue
   {
      private readonly List<JsonValue> m_values;

      public JsonArray(JsonValue[] a)
         : base(JsonValueTypes.ARRAY)
      {
         Contract.Requires(a != null);
         m_values = a.ToList();
      }

      [Pure]
      public int Length { get { return m_values.Count; } }

      [Pure]
      public int Count { get { return Length; } }

      [Pure]
      public JsonValue[] Value { get { return m_values.ToArray(); } }

      [Pure]
      public JsonValue[] Values { get { return Value; } }

      public JsonValue this[int i]
      {
         get
         {
            Contract.Requires(i >= 0);
            Contract.Requires(i < Count);
            return m_values[i]; 
         }
         set
         {
            Contract.Requires(i >= 0);
            Contract.Requires(i < Count);
            m_values[i] = value;
         }
      }

      public void Append(JsonValue v)
      {
         m_values.Add(v);
      }

      public override string MakePrintValue()
      {
         return string.Format(
             "[{0}]",
             string.Join(",", Values.Select(v => v.MakePrintValue()).ToArray()));
      }

      public override bool Equals(object obj)
      {
         var other = obj as JsonArray;

         return other != null
             && Value.Length == other.Values.Length
             && Enumerable.Range(0, this.Values.Length)
                 .All(i =>
                 {
                    var a = other.Values[i];
                    var b = this.Values[i];

                    return a.Equals(b);
                 });
      }

      public override int GetHashCode()
      {
         return Values.Aggregate(357, (i, json) => i ^ json.GetHashCode());
      }

      public static JsonArray Parse(string s)
      {
         var inputStream = new ANTLRStringStream(s);
         var lexer = new JsonLexer(inputStream);
         var tokens = new CommonTokenStream(lexer);
         var parser = new JsonParser(tokens);
         var parseTree = parser.array().Tree;
         var stream = new CommonTreeNodeStream(parseTree);
         var tree = new JsonTree(stream);

         var @object = tree.array();

         Contract.Assume(@object != null);
         return new JsonArray(JsonValueTypes.Interpret(@object));
      }

   }

   public class JsonObject : JsonValue
   {
      private readonly IDictionary<string, JsonValue> m_values;

      public JsonObject(IEnumerable<KeyValuePair<string, JsonValue>> pairs)
         : base(JsonValueTypes.OBJECT)
      {
         Contract.Requires(pairs != null);
         m_values = pairs
             .GroupBy(p => p.Key)
             .ToDictionary(g => g.Key, g => g.First().Value);
      }

      public JsonValue this[string s]
      {
         get { return m_values[s]; }
         set
         {
            if (m_values.ContainsKey(s))
            {
               m_values.Remove(s);
            }

            m_values.Add(s, value);
         }
      }
      public string[] Keys { get { return m_values.Keys.ToArray(); } }
      public bool Has(string memberName)
      {
         return Keys.Contains(memberName);
      }
      public IEnumerable<KeyValuePair<string, JsonValue>> Value { get { return m_values; } }
      public IEnumerable<KeyValuePair<string, JsonValue>> Pairs { get { return Value; } }
      public IDictionary<string, JsonValue> Dictionary { get { return m_values.ToDictionary(v => v.Key, v => v.Value); } }

      public void Add(string s, JsonValue v)
      {
         this[s] = v;
      }

      public void Add(KeyValuePair<string, JsonValue> kvp)
      {
         this[kvp.Key] = kvp.Value;
      }

      public override bool TryGetMember(GetMemberBinder binder, out object result)
      {
         var propName = binder.Name;
         if (Keys.Contains(propName))
         {
            result = m_values[propName];
            return true;
         }

         return base.TryGetMember(binder, out result);
      }

      public override bool TrySetMember(SetMemberBinder binder, object value)
      {
         this[binder.Name] = value is JsonValue ? (JsonValue)value : JsonValueTypes.Interpret(value);
         return true;
      }

      public override string MakePrintValue()
      {
         return string.Format(
             "{{{0}}}",
             string.Join(
                 ",",
                 Pairs.Select(kvp => string.Format(
                 "{0}:{1}",
                 JsonString.MakeStringValue(kvp.Key),
                 kvp.Value.MakePrintValue()))
                 .ToArray()));

      }

      public override bool Equals(object obj)
      {
         var other = obj as JsonObject;

         return other != null
             && Pairs.All(p => other.Keys.Contains(p.Key) && other[p.Key].Equals(p.Value));
      }

      public override int GetHashCode()
      {
         return Pairs.Aggregate(357, (i, kvp) => i ^ kvp.Key.GetHashCode() ^ kvp.Value.GetHashCode());
      }

      public static JsonObject Parse(string s)
      {
         var inputStream = new ANTLRStringStream(s);
         var lexer = new JsonLexer(inputStream);
         var tokens = new CommonTokenStream(lexer);
         var parser = new JsonParser(tokens);
         var parseTree = parser.@object().Tree;
         var stream = new CommonTreeNodeStream(parseTree);
         var tree = new JsonTree(stream);

         var @object = tree.@object();
         Contract.Assume(@object != null);
         return new JsonObject(JsonValueTypes.Interpret(@object));
      }

      [ContractInvariantMethod]
      private void ObjectContracts()
      {
         Contract.Invariant(this.m_values != null);
      }

   }


}
