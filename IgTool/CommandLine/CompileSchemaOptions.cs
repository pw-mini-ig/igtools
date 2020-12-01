using CommandLine;

namespace IgTool.CommandLine
{
    [Verb("compile-schema", HelpText = "Compiles IG YAML schema to JSON and validates it.")]
    public class CompileSchemaOptions
    {
        [Value(0, Required = true, HelpText = "YAML file with the schema to compile.")]
        public string Filename { get; set; }
        
        [Option(HelpText = "Where to save the resulting JSON file. Default is to save in the same" +
                           "directory as the YAML file, with the extension changed to .json")]
        public string Target { get; set; }
        
        [Option]
        public bool Verbose { get; set; }
    }
}