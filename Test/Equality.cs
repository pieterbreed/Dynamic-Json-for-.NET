using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DynamicJson;

namespace Test
{
    [TestClass]
    public class Equality
    {
        [TestMethod]
        public void ThatArraysEqualityWorks()
        {
            var a1 = JsonObject.Parse("{\"name\":[123, 234, 345, 456]}")["name"];
            var a2 = JsonObject.Parse("{\"name\":[123,234,345,456]}")["name"];
            var a3 = JsonObject.Parse("{\"name\":[123,234,345,4456]}")["name"];

            Assert.AreEqual(a1, a2);
            Assert.IsTrue(a1 == a2);
            Assert.IsTrue(a1 != a3);

        }

        [TestMethod]
        public void ThatObjectEqualityWorks()
        {
            var a1 = JsonObject.Parse("{\"name\":[123,234,345,456]}");
            var a2 = JsonObject.Parse("{\"name\":[123,234,345,456]}");
            var a3 = JsonObject.Parse("{\"name\":[123,234,345,4456]}");

            Assert.AreEqual(a1, a2);
            Assert.IsTrue(a1 != a3);
        }


    }
}
