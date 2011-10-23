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
    public class ThatAssignmentsGenerateKVPs
    {
        [TestMethod]
        public void ThatSingleAssignmentWorks()
        {
            var parser = Utilities.jsonParserFromString("\"key\":[1,2,3]");
            var tree = parser.pair().Tree;

            var stream = new CommonTreeNodeStream(tree);
            var walker = new JsonTree(stream);
            var root = walker.kvp();
            Assert.AreEqual("key", root.Key);
            Assert.AreEqual(typeof(object[]), root.Value.GetType());
            Assert.AreEqual(2d, ((object[])root.Value)[1]);

        }

        [TestMethod]
        public void ThatKvpFailOnNonStringKeys()
        {
            Action<object> tryToCreateKvp = k =>
                {
                    bool caught = false;

                    try
                    {
                        var parser = Utilities.jsonParserFromString(k.ToString() + ":[1,2,3]");
                        var tree = parser.pair().Tree;
                    }
                    catch(JsonException)
                    {
                        caught = true;
                    }
                    Assert.IsTrue(caught);
                };

            tryToCreateKvp(123);
            tryToCreateKvp(123d);
            tryToCreateKvp(new object[] { 1, 3});
        }
    }
}
