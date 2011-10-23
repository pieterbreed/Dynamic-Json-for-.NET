using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DynamicJson;

namespace Test
{
    [TestClass]
    public class DynamicUseCases
    {
        [TestMethod]
        public void SomeUseCasesWithoutDynamicProperties()
        {
            dynamic t = JsonObject.Parse(@"{""number"":123.345,""string"":""string value"",""array"":[123,234],""object"":{""object_nested"":146}}");
            Assert.AreEqual(JsonValueTypes.OBJECT, t.Type);

            dynamic number = t["number"];
            Assert.AreEqual(JsonValueTypes.NUMBER, number.Type);
            Assert.AreEqual(123.345, number);

            dynamic @string = t["string"];
            Assert.AreEqual(JsonValueTypes.STRING, @string.Type);
            Assert.AreEqual("string value", @string);

            dynamic array = t["array"];
            Assert.AreEqual(123d, array[0]);
            Assert.AreEqual(123, array[0]);
            Assert.AreEqual(234d, array[1]);
            Assert.AreEqual(234, array[1]);

            dynamic @object = t["object"];
            Assert.AreEqual(JsonValueTypes.OBJECT, @object.Type);
            Assert.AreEqual(1, @object.Keys.Length);
            Assert.AreEqual(146, t["object"]["object_nested"]);
        }

        [TestMethod]
        public void DynamicProperties()
        {
            dynamic t = JsonObject.Parse(@"{""number"":123.345,""string"":""string value"",""array"":[123,234],""object"":{""object_nested"":146}}");
            Assert.AreEqual(JsonValueTypes.OBJECT, t.Type);

            dynamic number = t.number;
            Assert.AreEqual(JsonValueTypes.NUMBER, number.Type);
            Assert.AreEqual(123.345, number);

            dynamic @string = t.@string;
            Assert.AreEqual(JsonValueTypes.STRING, @string.Type);
            Assert.AreEqual("string value", @string);

            dynamic array = t.array;
            Assert.AreEqual(123d, array[0]);    
            Assert.AreEqual(123, array[0]);
            Assert.AreEqual(234d, array[1]);
            Assert.AreEqual(234, array[1]);

            dynamic @object = t.@object;
            Assert.AreEqual(JsonValueTypes.OBJECT, @object.Type);
            Assert.AreEqual(1, @object.Keys.Length);
            Assert.AreEqual(146, t.@object.object_nested);
        }

        [TestMethod]
        public void DynamicPropertiesSet()
        {
            dynamic t = JsonObject.Parse("{}");
            t.@object = new JsonObject(new[]
                {
                    new KeyValuePair<string, JsonValue>("nested_object", new JsonNumber(123))
                });
            t.@string = "value";
            t.number = 123;
            t.@bool = true;
            t.@null = null;
            t.dyn_value = 123345;
            t.dict = new Dictionary<string, string>()
            {
                {"t1", "one"},
                {"t2", "two"},
                {"t3", "three"}
            };

            dynamic t2 = JsonObject.Parse(t.MakePrintValue());

            Assert.AreEqual("value", t2.@string);
            Assert.AreEqual(123, t2.number);
            Assert.AreEqual(true, t2.@bool);
            Assert.AreEqual(JsonNull.NULL, t2.@null);
            Assert.AreEqual(123, t2.@object.nested_object);
            Assert.AreEqual(123345, t2.dyn_value);

            Assert.AreEqual("one", t.dict.t1);
            Assert.AreEqual("two", t.dict.t2);
            Assert.AreEqual("three", t.dict.t3);
        }

    }
}
