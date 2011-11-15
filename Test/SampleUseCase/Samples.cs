using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using DynamicJson;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.SampleUseCase
{
   [TestClass]
   public class Samples
   {
      [TestMethod]
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
   }
}
