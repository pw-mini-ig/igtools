using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CommandLine;
using IgTool.CommandLine;
using IgTool.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace IgTool
{
    static class Program
    {
        static void Main(string[] args)
        {
            // TODO: maybe replace this with a more tightly-coupled module system?
            Parser.Default.ParseArguments<CompileSchemaOptions, ValidateOptions>(args)
                .WithParsed<CompileSchemaOptions>(CompileSchema)
                .WithParsed<ValidateOptions>(Validate);

            
            
            // string bdir = "Resources/";
            //
            // YamlTools.ConvertYamlToJson(bdir + "ig-schema.yaml", true);
            // var jsonMetaSchema = JsonTools.ReadSchema(bdir + "json-meta-schema.json");
            // var igSchemaJson = JsonTools.ReadJson(bdir + "ig-schema.json");
            // bool valid = igSchemaJson.IsValid(jsonMetaSchema, out var messages); 
            //
            // var igSchema = JsonTools.ReadSchema(bdir + "ig-schema.json");
            

            /*const string test = "hopsa hopsa od sasa do lasa";

            var root = new NodeDocument
            {
                SpanLength = test.Length,
                SpanStart = 0,
                Text = test
            };
            var child1 = new Node(root)
            {
                Text = "hopsa od sasa"
            };
            var child11 = new Node(child1)
            {
                Text = "hopsa",
                SpanStart = 0,
                SpanLength = 5
            };
            var tree = new Tree(test, root);

            Console.WriteLine(tree);

            while (true)
            {
                var status = tree.InspectTree();
                Console.WriteLine();
                Console.WriteLine(status);
                if (status.IsOk || status.AutofixStatus != TreeInspectionAutofixStatus.Possible)
                    break;

                Console.WriteLine("Autofixing...");
                status.PossibleFixes.First().Apply();
                Console.WriteLine(tree);
            }*/

            //Console.ReadLine();
        }

        private static void CompileSchema(CompileSchemaOptions opt)
        {
            if (opt.Verbose) Console.WriteLine("Converting YAML to JSON...");
            string savedJson;
            
            // Convert to JSON
            try
            {
                savedJson = YamlTools.ConvertYamlToJson(
                    opt.Filename, opt.Target, 
                    YamlOptions.StringsToBools | YamlOptions.StringsToInts
                );
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error converting YAML to JSON:");
                Console.Error.WriteLine(ex.Message);
                return;
            }
            
            // Validate
            var jsonMetaSchema = JsonTools.ReadSchema("res/json-meta-schema.json");
            var igSchemaJson = JsonTools.ReadJson(savedJson);
            bool valid = igSchemaJson.IsValid(jsonMetaSchema, out var messages);
            if (!valid)
            {
                Console.Error.WriteLine("The schema is invalid:");
                foreach (var errorMessage in messages)
                    Console.Error.WriteLine(errorMessage);
                return;
            }
            if (opt.Verbose) Console.WriteLine("The schema is valid.");
            if (opt.Verbose) Console.WriteLine($"Done. Resulting JSON was saved as {savedJson}");
        }

        private static void Validate(ValidateOptions opt)
        {
            IEnumerable<string> jsons;
            
            // Load file
            try
            {
                if (opt.Filename.EndsWith(".json"))
                    using (var sr = new StreamReader(opt.Filename))
                        jsons = new []{ sr.ReadToEnd() };
                else jsons = YamlTools.ReadYamlFileToJsonStrings(opt.Filename);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error reading file:");
                Console.Error.WriteLine(ex.Message);
                return;
            }
            
            // Validate
            int counter = 0, validCounter = 0;
            foreach (var json in jsons)
            {
                var schema = JsonTools.ReadSchema(opt.Schema);
                var jsonObject = JObject.Parse(json);
                bool valid = jsonObject.IsValid(schema, out var messages);
                if (!valid)
                {
                    Console.Error.WriteLine($"Document #{counter + 1} is invalid:");
                    foreach (var errorMessage in messages)
                        Console.Error.WriteLine(errorMessage);
                }
                else validCounter++;

                counter++;
            }

            Console.WriteLine($"Valid documents in file: {validCounter} / {counter}.");
        }
    }
}
