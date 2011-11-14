using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace DynamicJson
{
   public static class Utilities
   {

      public static string GetStringValueFromToken(string tokenValue)
      {
         if (string.IsNullOrEmpty(tokenValue)
             || tokenValue.Length < 2
             || tokenValue[0] != '"'
             || tokenValue[tokenValue.Length - 1] != '"')
         {
            throw new InvalidOperationException("the string token value does not have quotes on either end");

         }
         return tokenValue
             .Substring(1, tokenValue.Length - 2)
             .JsonUnescape();
      }

      public static string JsonUnescape(this string str)
      {
         var result = str
             .Replace("\\\"", @"""")
             .Replace(@"\\", @"\")
             .Replace(@"\/", "/")
             .Replace(@"\b", "\b")
             .Replace(@"\f", "\f")
             .Replace(@"\n", "\n")
             .Replace(@"\r", "\r")
             .Replace(@"\t", "\t");

         var regex = new Regex(@"\\u([0-9a-fA-F]{4})");
         while (true)
         {
            var match = regex.Match(result);
            if (!match.Success) break;

            var ch = (char)int.Parse(match.Groups[1].Value, NumberStyles.HexNumber);
            var oldValue = "\\u" + match.Groups[1];
            if (oldValue.Length > 0)
            {
               result = result.Replace(oldValue, new string(new char[] { ch }));
            }
         }

         return result;
      }


   }
}
