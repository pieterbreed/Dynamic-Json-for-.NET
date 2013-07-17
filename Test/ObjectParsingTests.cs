using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Antlr.Runtime.Tree;
using DynamicJson;

namespace Test
{
    [TestClass]
    public class ObjectParsingTests
    {
       [TestMethod]
       public void ThatStringsParse()
       {
          var jsonStr = JsonValue.Parse(@"""""");
       }

       [TestMethod]
       public void TestThatEscapeTestWorks()
       {
          var result = DynamicJson.Utilities.PerformOneEscapeStep(
             "\\t",
             "\t",
             new[]
                {
                   "aaa\\tbbb\\tccc", "\\tddd"
                });

          Assert.IsTrue(
             new[]
                {
                   "aaa", "\t", "bbb", "\t", "ccc", "", "\t", "ddd"
                }.SequenceEqual(result),
             "escape step not behaving");
       }

       [TestMethod]
       public void ThatStringsEscapesProperly()
       {
          var escapedString = @"""\tab\newline\\backslash\\tab-in-3-letters""";
          var js = JsonValue.Parse(escapedString);

          Assert.IsTrue(js.IsString);
          Assert.AreEqual("\tab\newline\\backslash\\tab-in-3-letters", (string) js);
       }

      [TestMethod]
       public void ThatStringsDoesntParseAsObject2()
      {
         var str =
            @"{
            ""AssetClass"": ""Equity"",
            ""AvailableFromDate"": ""/Date(1125446400000)/"",
            ""Currency"": ""ZAR"",
            ""ID"": ""TEGN"",
            ""InceptionDate"": ""/Date(1125446400000)/"",
            ""Name"": ""Australia--Equity--General"",
            ""Region"": """",
            ""Number"": ""123.123"",
            ""Number2"": 123.123,
            ""ShareID"": ""0""
        }";
         var json = JsonObject.Parse(str);
         Assert.IsTrue(json["Region"].IsString);
         Assert.IsTrue(json["Number"].IsString);
         Assert.IsTrue(json["Number2"].IsNumber);

      }

        [TestMethod]
        public void ThatEmptyObjectParses()
        {
            var parser = Utilities.jsonParserFromString("{}");
            var tree = parser.@object().Tree;

            var stream = new CommonTreeNodeStream(tree);
            var walker = new JsonTree(stream);
            var root = walker.@object();

            Assert.AreEqual(typeof(KeyValuePair<string, object>[]), root.GetType());
            Assert.AreEqual(0, root.Length);

        }

        [TestMethod]
        public void ObjectWithSimplePair()
        {
            var parser = Utilities.jsonParserFromString("{\"testKey\":123.123}");
            var tree = parser.@object().Tree;

            var stream = new CommonTreeNodeStream(tree);
            var walker = new JsonTree(stream);
            var root = walker.@object();


            Assert.AreEqual(typeof(KeyValuePair<string, object>[]), root.GetType());
            Assert.AreEqual(1, root.Length);
            Assert.AreEqual("testKey", root[0].Key);
            Assert.AreEqual(123.123, root[0].Value);
        }

        [TestMethod]
        public void ObjectWith2SimplePairs()
        {
            var parser = Utilities.jsonParserFromString("{\"testKey\":123.123,\"key2\":\"string value\"}");
            var tree = parser.@object().Tree;

            var stream = new CommonTreeNodeStream(tree);
            var walker = new JsonTree(stream);
            var root = walker.@object();


            Assert.AreEqual(typeof(KeyValuePair<string, object>[]), root.GetType());
            Assert.AreEqual(2, root.Length);
            Assert.AreEqual("testKey", root[0].Key);
            Assert.AreEqual(123.123, root[0].Value);
            Assert.AreEqual("key2", root[1].Key);
            Assert.AreEqual("string value", root[1].Value);
        }

        [TestMethod]
        public void ObjectWithObjectAndArrayValues()
        {
            //var parser = Utilities.jsonParserFromString("{\"testKey\":[123],\"key2\":{},\"key3\":{\"key4\":234}}");
            var parser = Utilities.jsonParserFromString("{\"testKey\":[123],\"key2\":{}}");
            var tree = parser.toplevel().Tree;

            var stream = new CommonTreeNodeStream(tree);
            var walker = new JsonTree(stream);
            var root = walker.@object();


            Assert.AreEqual(typeof(KeyValuePair<string, object>[]), root.GetType());
            Assert.AreEqual(2, root.Length);
            Assert.AreEqual("testKey", root[0].Key);
            Assert.IsTrue(new object[] { 123d }.All(o => (root[0].Value as object[]).Contains(o)));
            Assert.AreEqual("key2", root[1].Key);
            Assert.AreEqual(typeof(KeyValuePair<string, object>[]), root[1].Value.GetType());
            Assert.AreEqual(0, (root[1].Value as KeyValuePair<string, object>[]).Length);
        }

        [TestMethod]
        public void ThatWhiteSpaceIsIgnored()
        {
            var parser = Utilities.jsonParserFromString("   \n{\"testKey\"\n:[123],\t   \"key2\"  :   {}\n\n\n}   ");
            var tree = parser.toplevel().Tree;

            var stream = new CommonTreeNodeStream(tree);
            var walker = new JsonTree(stream);
            var root = walker.@object();

            Assert.AreEqual(typeof(KeyValuePair<string, object>[]), root.GetType());
            Assert.AreEqual(2, root.Length);
            Assert.AreEqual("testKey", root[0].Key);
            Assert.IsTrue(new object[] { 123d }.All(o => (root[0].Value as object[]).Contains(o)));
            Assert.AreEqual("key2", root[1].Key);
            Assert.AreEqual(typeof(KeyValuePair<string, object>[]), root[1].Value.GetType());
            Assert.AreEqual(0, (root[1].Value as KeyValuePair<string, object>[]).Length);

        }
    }
}
