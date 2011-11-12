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
         var now = DateTime.Now;
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

   }
}
