using System;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using DynamicJson;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
   [TestClass]
   public class AspTests
   {
      [TestMethod]
      public void ThatJsonDateTimeRoundtrips()
      {
         for (DateTime d = DateTime.Now.AddDays(-5).Date; d < DateTime.Now.Date; d = d.AddDays(1))
         {
            CheckDateRoundtrips(d);   
         }
      }

      private static void CheckDateRoundtrips(DateTime now)
      {
         var ms = (now - new DateTime(1970, 1, 1)).Ticks / 10000;
         var serializedNow = AspTools.SerializeDateTimeToString(now);
         Assert.AreEqual(string.Format("/Date({0})/", ms), serializedNow);

         var jsonString = new JsonString(serializedNow);
         Debug.WriteLine(jsonString.ToString());
         var dNow = AspTools.ParseStringToDateTime(jsonString);
         Assert.AreEqual(
            new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond),
            new DateTime(dNow.Year, dNow.Month, dNow.Day, dNow.Hour, dNow.Minute, dNow.Second, dNow.Millisecond));
      }

      [TestMethod]
      public void ThatStringIsDateWorks()
      {
         Assert.IsFalse(AspTools.StringIsDate("aoeu"));
         Assert.IsFalse(AspTools.StringIsDate("123"));
         Assert.IsFalse(AspTools.StringIsDate("2001-09-31"));

         Assert.IsTrue(AspTools.StringIsDate("/Date(1320451200000)/"));
      }

      [TestMethod]
      public void ThatNegativeDateTimesWorks()
      {
         Assert.AreEqual(AspTools.ParseStringToDateTime(new JsonString("/Date(-20476800000)/")), new DateTime(1969, 5, 9, 0, 0, 0));
      }
   }
}
