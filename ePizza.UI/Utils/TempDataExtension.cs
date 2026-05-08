using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ePizza.UI.Utils
{
    public static class TempDataExtension
    {
        public static void Set<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            // need to check this usage of JsonSerializerOptions in internet

            // when you pass the json object , it useful to ignore the circular references, if don't write this it will throw exception for infinite loop

            JsonSerializerOptions option
                  = new JsonSerializerOptions()
                  {
                      ReferenceHandler = ReferenceHandler.IgnoreCycles
                  };

            tempData[key] = JsonSerializer.Serialize(value, option);

        }

        public static T Peek<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            object obj = tempData.Peek(key);
            return obj == null ? null : JsonSerializer.Deserialize<T>((string)obj);
        }

    }
}
