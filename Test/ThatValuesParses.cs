using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Antlr.Runtime.Tree;

namespace Test
{
    [TestClass]
    public class ThatValuesParses
    {
        [TestMethod]
        public void ThatStringUnEscapingWorks()
        {
            Action<char, string> assertCharParses = (c, str) =>
                {
                    Assert.AreEqual(new string(new char[] { c }), DynamicJson.Utilities.JsonUnescape(str));
                };

            assertCharParses('\\', @"\\");
            assertCharParses('"', @"\""");
            assertCharParses('/', @"\/");
            assertCharParses('\b', @"\b");
            assertCharParses('\f', @"\f");
            assertCharParses('\n', @"\n");
            assertCharParses('\r', @"\r");
            assertCharParses('\t', @"\t");

            foreach (char c in "aoeuntahoe snthaoentyhs taohentu ".ToArray<char>())
            {
                assertCharParses(c, new string(new char[] { c }));
            }

            assertCharParses('\u1234', @"\u1234");

            Assert.AreEqual(new string(new char[] { '\u4321', '\u3342', '\u5544' }), "\u4321\u3342\u5544");
            
        }

        [TestMethod]
        public void ThatStringGetTokenValueWorks()
        {
            Action<string> expectFailure = str =>
                {
                    bool caught = false;
                    try
                    {
                        DynamicJson.Utilities.GetStringValueFromToken(str);
                    }
                    catch (InvalidOperationException)
                    {
                        caught = true;
                        // swallow since this is what we want
                    }
                    Assert.IsTrue(caught, "exception not caught");
                };

            expectFailure(null);
            expectFailure("");
            expectFailure("oeu");
            expectFailure("\"");
            expectFailure("u");

            var tries = new[] {
                "thaoeu",
                "is",
                "just",
                "\u1234",
                "some testing",
                "aoeu \u4321 \u4543 that is to be happening"
            };

            Assert.IsTrue(tries
                .Select(str => new
                {
                    Orig = str,
                    Extr = DynamicJson.Utilities.GetStringValueFromToken("\"" + str + "\"")
                })
                .All(item => item.Extr == item.Orig));

        }

        [TestMethod]
        public void ThatStringsParse()
        {
            var parser = Utilities.jsonParserFromString("\"1\\\"23\"");
            var tree = parser.value().Tree;
            var t = (new DynamicJson.JsonTree(new CommonTreeNodeStream(tree))).@string();
            Assert.AreEqual("1\"23", t);

        }
    }
}
