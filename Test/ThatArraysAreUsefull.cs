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
    public class ArrayTests
    {
        [TestMethod]
        public void ThatArraysHasNumbers()
        {
            var parser = Utilities.jsonParserFromString("[123,234,345]");
            var tree = parser.array().Tree;

            var stream = new CommonTreeNodeStream(tree);
            var walker = new JsonTree(stream);
            var root = walker.array();

            Assert.AreEqual(123d, root[0]);
            Assert.AreEqual(234d, root[1]);
            Assert.AreEqual(345d, root[2]);

        }

        [TestMethod]
        public void ThatArraysCanBeEmpty()
        {
            var str = "[]";
            var parser = Utilities.jsonParserFromString(str);
            var tree = parser.array().Tree;

            var stream = new CommonTreeNodeStream(tree);
            var walker = new JsonTree(stream);
            var root = walker.array();
            Assert.AreEqual(0, root.Length);

        }

        [TestMethod]
        public void ThatArraysCanHaveStrings()
        {
            var str = "[\"oeu\",\"uu\"]";
            var parser = Utilities.jsonParserFromString(str);
            var tree = parser.array().Tree;

            var stream = new CommonTreeNodeStream(tree);
            var walker = new JsonTree(stream);
            var root = walker.array();
            Assert.AreEqual(2, root.Length);
            Assert.AreEqual("oeu", root[0]);
            Assert.AreEqual("uu", root[1]);

        }


        [TestMethod]
        public void ThatArraysCanHaveArrays()
        {
            var parser = Utilities.jsonParserFromString("[123,[],[654,888],345]");
            var tree = parser.array().Tree;

            var stream = new CommonTreeNodeStream(tree);
            var walker = new JsonTree(stream);
            var root = walker.array();
            Assert.AreEqual(4, root.Length);
            Assert.AreEqual(123d, root[0]);
            Assert.AreEqual(345d, root[3]);

            var a1 = root[1];
            var a2 = root[2];
            Assert.AreEqual(typeof(object[]), a1.GetType());
            Assert.AreEqual(typeof(object[]), a2.GetType());

            Assert.AreEqual(0, ((object[])a1).Length);
            Assert.AreEqual(2, ((object[])a2).Length);

            var o2 = (object[])a2;
            Assert.AreEqual(654d, o2[0]);
            Assert.AreEqual(888d, o2[1]);

        }


        [TestMethod]
        public void ThatArraysCanHaveBooleansAndNull()
        {
            var parser = Utilities.jsonParserFromString("[true,true,false,true,false,null]");
            var tree = parser.array().Tree;

            var stream = new CommonTreeNodeStream(tree);
            var walker = new JsonTree(stream);
            var root = walker.array();


            Assert.AreEqual(6, root.Length);
            Assert.IsTrue((bool)root[0]);
            Assert.IsTrue((bool)root[1]);
            Assert.IsTrue((bool)root[3]);
            Assert.IsFalse((bool)root[2]);
            Assert.IsFalse((bool)root[4]);
            Assert.IsNull(root[5]);
        }

       [TestMethod]
       public void ThatCountAndLengthWork()
       {
          var array = JsonArray.Parse("[1,3,5,7]");
          Assert.IsTrue(array.Type.Equals(JsonValueTypes.ARRAY));
          Assert.AreEqual(4, array.Length);
          Assert.AreEqual(4, array.Count);
       }

    }
}
