using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DynamicJson;

namespace Test
{
    [TestClass]
    public class Printing
    {
        [TestMethod]
        public void ThatJsonValuesCanBePrintedToString()
        {
            var orig = "{\"name\":123.234}";
            var v = JsonObject.Parse(orig);
            var result = v.MakePrintValue();
            Assert.AreEqual(orig, result);
        }

        [TestMethod]
        public void ThatJsonValuesCanBePrintedToString2()
        {
            var orig = "{\"name\":123.234,\"name_two\":[123,234],\"object\":{\"o_one\":123}}";
            var v = JsonObject.Parse(orig);
            var result = v.MakePrintValue();
            Assert.AreEqual(orig, result);
        }

        [TestMethod]
        public void TestTwoWayPrintingAndParsing()
        {
            var orig = "{\"name\":123.234,\"name_two\":[123,234],\"object\":{\"o_one\":123}}";
            dynamic v = JsonObject.Parse(orig);
            var v_str = v.MakePrintValue();
            dynamic v2 = JsonObject.Parse(v_str);

            Assert.AreEqual(v.Dictionary["name"], v2.Dictionary["name"]);
            Assert.AreEqual(v["name_two"][0], v2["name_two"][0]);
            Assert.AreEqual(v["name_two"][1], v2["name_two"][1]);

            Assert.AreEqual(v["object"]["o_one"], v2["object"]["o_one"]);
        }

    }
}
