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

      /// <summary>
      /// this ugly method performs one escape sequence step. if it is called with 
      /// (@"\t", "\t", ["aaa\\tbbb\\tccc", "\\tddd"]) it will return [ "aaa", "\t", "bbb", "\t", "ccc", "\t", "ddd" ]
      /// </summary>
      /// <param name="from">the sequence that must be replaced</param>
      /// <param name="to">what it must be replaced with</param>
      /// <param name="inputs">an enumerable of strings that must all have the escape performed on them</param>
      /// <returns>an enumerable of strings that will isolate the just-escaped strings so that they won't interfere with escapes coming up</returns>
      public static IEnumerable<string> PerformOneEscapeStep(string from, string to, IEnumerable<string> inputs)
      {
         var tempResult = new List<string>();

         foreach (var input in inputs)
         {
            if (!input.Contains(from))
            {
               tempResult.Add(input);
               continue;
            }

            var splits = input.Split(
               new[]
                  {
                     from
                  },
               StringSplitOptions.None);
            var times = splits.Count() - 1;
            tempResult.AddRange(
               splits.Zip(
                  Enumerable.Repeat(
                     to,
                     times),
                  (a, b) => new[]
                     {
                        a, b
                     })
                  .SelectMany(x => x)
                  .Concat(
                     new[]
                        {
                           splits[times]
                        }));
         }
         return tempResult;
      }

      public static string JsonUnescape(this string str)
      {
         var escapes = new[]
            {
               new[]
                  {
                     "\\\"", @""""
                  },
               new[]
                  {
                     @"\\", @"\"
                  },
               new[]
                  {
                     @"\/", "/"
                  },
               new[]
                  {
                     @"\b", "\b"
                  },
               new[]
                  {
                     @"\f", "\f"
                  },
               new[]
                  {
                     @"\n", "\n"
                  },
               new[]
                  {
                     @"\r", "\r"
                  },
               new[]
                  {
                     @"\t", "\t"
                  }
            };

         IEnumerable<string> handle = new string[] { str };
         handle = escapes.Aggregate(handle, (current, escape) => PerformOneEscapeStep(escape[0], escape[1], current));
         var result = string.Join(
            string.Empty,
            handle);

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
