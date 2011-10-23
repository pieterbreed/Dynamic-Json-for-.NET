README
======

What
----

Dynamic Json is a C# library for parsing JSON strings into CLR objects for which you don't have to pre-define object structures.
It supports discovery of the JSON objects and types and is also very suitable for use with the C# 4 dynamic type.

How
---

[ANTLR](http://www.antlr.org/) is used for parsing the JSON text.

You can use it by referencing these dlls into your project:

* Antlr3.Runtime.dll
* DynamicJson.dll 

Typical use-cases are shown below in the why section.

Why
---

There are a lot of these JSON libraries around, but I wanted to use something which does not require you to pre-define a class
beforehand. JSON is in its nature very dynamic and I wanted something that would preserve this dynamicity. To illustrate the point,
here is one of the unit tests that shows the typical usage scenario that I had in mind.

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
			
here is another example:

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