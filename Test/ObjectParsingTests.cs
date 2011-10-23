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
