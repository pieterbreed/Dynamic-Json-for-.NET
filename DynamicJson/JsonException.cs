using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DynamicJson
{
    [Serializable]
    public class JsonException : Exception
    {
        public JsonException() { }
        public JsonException(string message) : base(message) { }
        public JsonException(string message, Exception inner) : base(message, inner) { }
        protected JsonException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

   [Serializable]
   public class AspDateTimeException : ArgumentException
   {
      public AspDateTimeException()
      {
      }

      public AspDateTimeException(string message) : base(message)
      {
      }

      public AspDateTimeException(string message, Exception inner) : base(message, inner)
      {
      }

      protected AspDateTimeException(
         SerializationInfo info,
         StreamingContext context) : base(info, context)
      {
      }
   }
}
