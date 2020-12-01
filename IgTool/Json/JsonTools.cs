using System;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace IgTool.Json
{
    public static class JsonTools
    {
        public static JsonSchema ReadSchema(string file)
        {
            using var sr = new StreamReader(file);
            
            var text = sr.ReadToEnd();
            return JsonSchema.Parse(text);
        }

        public static JObject ReadJson(string file)
        {
            using var sr = new StreamReader(file);
            return JObject.Parse(sr.ReadToEnd());
        }

        // public static bool ValidateJson(string file)
        // {
        //     
        // }
    }
}