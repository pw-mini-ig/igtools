using CommandLine;

namespace IgTool.CommandLine
{
    [Verb("validate", HelpText = "Validates the given IG YAML or JSON against IG schema.")]
    public class ValidateOptions
    {
        [Value(0, Required = true, HelpText = "YAML or JSON file to be validated.")]
        public string Filename { get; set; }
        
        [Option(Default="res/ig-schema.json",
            HelpText = "JSON file containing the compiled JSON IG schema. If not provided," + 
                       "IG schema packaged with the tool will be used.")]
        public string Schema { get; set; }

        [Option]
        public bool Verbose { get; set; }
    }
}