using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterCommonExtensions
{
    public static class JsonConverter
    {
        public static T ConvertToObject<T>(string jsonRequest)
        {
            T result = JsonConvert.DeserializeObject<T>(jsonRequest);
            return result;
        }
        public static string ConvertToJson(object value)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,

            };
            return JsonConvert.SerializeObject(value, settings);
        }
    }
}
