using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
   [TestClass]
   public class Casting
   {
      [TestMethod]
      public void ThatCastingWorks()
      {
         var val = DynamicJson.JsonObject.Parse(@"{""int"":123,""double"":123.123,""string"": ""well hello there"", ""boolean"":true}");
         Assert.AreEqual(123, (int)val["int"]);
         Assert.AreEqual(123.123, (double)val["double"]);
         Assert.AreEqual("well hello there", (string)val["string"]);
         Assert.AreEqual(true, (bool)val["boolean"]);
      }
   }
}
