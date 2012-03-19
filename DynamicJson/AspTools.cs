using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics.Contracts;

namespace DynamicJson
{
   /// <summary>
   /// provides tools to work nicely in a ASP environment
   /// </summary>
   public static class AspTools
   {
      private static readonly string DATETIME_REGEX_STR = @"/Date\(([-]?\d+\+\d\d\d\d|[-]?\d+)\)/";
      private static readonly Regex PARSE_DATETIME_STR = new Regex(DATETIME_REGEX_STR);

      /// <summary>
      /// parses a string in the format '\/Date(700000+0500)\/' to a datetime value.
      /// </summary>
      /// <param name="str">must be a non-null, valid serialized datetime string</param>
      /// <returns>Valid datetime</returns>
      /// <exception cref="AspDateTimeException">throws a AspDateTimeException if the str is not in the correct format</exception>
      /// <exception cref="ArgumentException">thrown if str is null</exception>
      /// <see cref="http://msdn.microsoft.com/en-us/library/bb412170.aspx"/>
      public static DateTime ParseStringToDateTime(JsonString str)
      {
         Contract.Requires(str != null);

         var match = PARSE_DATETIME_STR.Match(str);

         if (!match.Success) throw new AspDateTimeException(string.Format("'{0}' is not a valid format ({1})", str.Value, DATETIME_REGEX_STR));

         var matchedValue = match.Groups[1].Value;
         bool isUtc = true;
         var indexOf = matchedValue.IndexOf('+');
         if (indexOf > -1)
         {
            isUtc = false;
            matchedValue = matchedValue.Substring(0, indexOf);
         }

         try
         {
            var ms = long.Parse(matchedValue);
            if (ms > 0)
            {
               return new DateTime(1970, 1, 1, 0, 0, 0, isUtc ? DateTimeKind.Utc : DateTimeKind.Local)
                     + new TimeSpan(ms * 10000);
            }
            else
            {
               return new DateTime(1970, 1, 1, 0, 0, 0, isUtc ? DateTimeKind.Utc : DateTimeKind.Local)
                     - new TimeSpan(-ms * 10000);
            }
            
         }
         catch (FormatException fe)
         {
            throw new AspDateTimeException("datetime string is not in the correct format; cannot parse to long", fe);
         }
      }

      public static bool StringIsDate(string s)
      {
         bool result = true;
         try
         {
            var date = ParseStringToDateTime(new JsonString(s));
         }
         catch (AspDateTimeException e)
         {
            result = false;
         }
         return result;
      }

      public static string SerializeDateTimeToString(DateTime d)
      {
         var ms = Convert.ToInt64(
            Math.Floor(d
                  .Subtract(new DateTime(1970, 1, 1))
                  .TotalMilliseconds));
         return string.Format("/Date({0})/", ms);
      }
   }
}
