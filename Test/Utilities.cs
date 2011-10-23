using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamicJson;
using Antlr.Runtime;
using Antlr.Runtime.Tree;

namespace Test
{
    static class Utilities
    {
        public static JsonParser jsonParserFromString(string input)
        {
            var inputStream = new ANTLRStringStream(input);
            var lexer = new JsonLexer(inputStream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new JsonParser(tokens);
            return parser;
        }

        public static JsonTree jsonTreeFromString(string input)
        {
            var parser = jsonParserFromString("\"123\"");
            var tree = parser.@object().Tree;

            var stream = new CommonTreeNodeStream(tree);
            return new JsonTree(stream);
        }




    }
}
