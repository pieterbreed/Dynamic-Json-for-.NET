using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
   [TestClass]
   public class EmptyArraysAsMembers
   {
      [TestMethod]
      public void TestMethod1()
      {
         var parsed = DynamicJson.JsonObject.Parse("{\"sections\":[]}");
         Assert.AreEqual(typeof(DynamicJson.JsonArray), parsed["sections"].GetType());
      }
   }
}
