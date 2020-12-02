using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using YamlDotNet.Serialization;

namespace IgTool.Json
{
    [Flags]
    public enum YamlOptions
    {
        None = 0x00,
        StringsToBools = 0x01,
        StringsToInts = 0x02,
        PrettyPrint = 0x04
    }
    
    public static class YamlTools
    {
        private static readonly Regex IntegerRegex = new Regex(@"""(\d+)""", RegexOptions.Compiled);
        
        public static string ConvertYamlToJson(string file, string target, YamlOptions options = YamlOptions.None)
        {
            file = file.Trim();

            if (target == null)
            {
                if (file.EndsWith(".yaml"))
                    target = file.Substring(0, file.Length - 4) + "json";
                else if (file.EndsWith(".yml"))
                    target = file.Substring(0, file.Length - 3) + "json";
                else throw new ArgumentException("Should point to a .y(a)ml file", nameof(file));
            }

            var jsons = ReadYamlFileToJsonStrings(file, options).ToArray();
            if (jsons.Length != 1)
                throw new Exception("YAML schema should contain exactly one document.");

            using var sw = new StreamWriter(target);
            sw.Write(jsons[0]);
            return target;
        }

        public static IEnumerable<string> ReadYamlFileToJsonStrings(string file, YamlOptions options = YamlOptions.None)
        {
            var deserializer = new DeserializerBuilder().Build();
            string yaml;
            using (var sr = new StreamReader(file))
                yaml = sr.ReadToEnd();
            
            // Sometimes I get into the mood for functional programming...
            var yamlObjects = (yaml ?? "")
                .Split("\n---", StringSplitOptions.RemoveEmptyEntries)
                .Select(doc => new StringReader(doc))
                .Select(sr => deserializer.Deserialize(sr))
                .Where(x => x != null)
                .ToArray();

            if (yamlObjects.Length == 0)
                throw new Exception("Retrieved no data from the YAML file.");

            var serializer = new SerializerBuilder()
                .JsonCompatible()
                .Build();

            var res = yamlObjects.Select(yo => serializer.Serialize(yo));
            
            if (options.HasFlag(YamlOptions.StringsToBools))
            {
                // TODO: FIX THIS ASAP
                // THE SCHEMA HAS BOOLEAN FIELDS AND THIS WILL BREAK FOR SURE
                //
                // HACK: replace all occurrences of "true", "True", "false" and "False"
                // with true and false (literal values). This is needed due to YAML being
                // really ambiguous with bools. It also doesn't help that this library doesn't really
                // support bools properly: https://github.com/aaubry/YamlDotNet/issues/387
                res = res.Select(json => json
                    .Replace("\"true\"", "true", true, null)
                    .Replace("\"false\"", "false", true, null)
                );
            }

            if (options.HasFlag(YamlOptions.StringsToInts))
            {
                // MORE HACKS:
                // Convert strings to integers. For the same reason as above. Sigh.
                res = res.Select(json => 
                    IntegerRegex.Replace(json, match => match.Groups[1].Value));
            }

            return res;
        }
    }
}