README
======

Status
------

Nov 2011: This library is in use on client-facing production code at my employer. It was developed in preparation for this
project and is being updated as I'm seeing new use-cases. The current major version is 0.9 with small updates
increasing the minor version number sequentially. The updates so far don't break code written against
older version, but rather addsnew convenience features.

What
----

Dynamic Json is a C# library for parsing JSON strings into CLR objects for which you don't have to pre-define object structures.
It supports discovery of the JSON objects and types and is also very suitable for use with the C# 4 dynamic type.

### What is it good for?

JSON is dynamic technology where C# started out as statically typed, OO technology. 

There is an obvious impedance mismatch here because as C# developers we got lulled into thinking that
all data must conform to pre-defined structure. We are very used to saying 'this is what my data will always look like'
even when that doesn't have to be the case.

For example: WCF is an awesome technology, writing REST services have never been so easy, but data enters
the system only once it's been serialized into objects via classed marked with DataContract attributes, 
which is essentially the same user-story that we've had for serialization since the early days.

JSON on the other hand comes from JavaScript which is a dynamically typed language. Data can enter the
system any way it wants to. 

An example of the kind of problem that is being discussed here is when a report can be broken up into
sections. An example of a section is a paragraph, or one kind of chart, or another kind of chart and a table.

None of those types of sections are logically the same type of thing, but they belong together and are
interchangable (can come in any order). While you can invent a structure that knows how to deal with this
situation I'm saying you might not have to and this library can help you in this situation.


How
---
djson is licensed under the [MIT license](http://www.opensource.org/licenses/mit-license.php), 
which basically means you may use it for anything you'd like. A thank-you would be nice.

[ANTLR](http://www.antlr.org/) is used for parsing the JSON text.

You can use it by referencing these dlls into your project:

* Antlr3.Runtime.dll
* DynamicJson.dll 

Alternatively you can reference the DynamicJsonForDotNET NuGet package which will set these references for you

Typical use-cases are shown below in the why section.

Typical example of what it's good at
------------------------------------

Contents of SampleData.json

		{
		   "dynamicDataUseCase": 
			  {
				 "useCase1": "this is a string value",
				 "useCase2": 
					{
					   "format": "this is the format function with {0} extra {1} parameters {2}",
					   "params": [123, "testing", true]
					}
			  }
		}

code using this data:

      public void ShowingDynamicDataUseCase()
      {
         var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Test.SampleUseCase.SampleData.json");
         Assert.IsNotNull(stream);
         var dataString = new StreamReader(stream).ReadToEnd();
         
         dynamic data = JsonObject.Parse(dataString)["dynamicDataUseCase"]; // of type JsonValue

         var useCase1 = data.useCase1; 
         var useCase2 = data.useCase2;

         Assert.AreEqual("this is a string value", HandleDynamicValue(useCase1));
         Assert.AreEqual("this is the format function with 123 extra testing parameters True", HandleDynamicValue(useCase2));
      }

      private static string HandleDynamicValue(dynamic useCase)
      {
         var result = string.Empty;

         if (useCase.IsString)
         {
            result = useCase.Value;
         }
         else if (useCase.IsObject && useCase.Has("format") && useCase.Has("params")
                  && useCase.@params.IsArray)
         {
            var @params = ((JsonArray) useCase.@params)
               .Values.Select(value => ((dynamic) value).Value)
               .ToArray();

            result =
               string.Format(
                  useCase.format,
                  @params);
         }

         return result;
      }

Why
---

There are [a lot of these JSON libraries around](http://json.org/), but I wanted to use something which does not require you to pre-define a class
beforehand. JSON is in its nature very dynamic and I wanted something that would preserve this dynamicity when it
is being consumed. To illustrate the point:

Here are two unit tests showing the usage scenarios I had in mind when I started this project:

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
			
ASP Environments
----------------

There is special support for the way that datetime values are [serialized in ASP (WCF) environments](http://msdn.microsoft.com/en-us/library/bb412170.aspx).

You use it with the AspTools: string SerializeDateTimeToString(DateTime d) and AspTools: DateTime ParseStringToDateTime(JsonString str) methods.

What's New
----------

0.9.5
=====

- Contracts
- casts from JsonValue to various numeric value types, string, boolean
- IsString, IsNumber, IsBool, IsArray, IsObject, IsNull on JsonValue
