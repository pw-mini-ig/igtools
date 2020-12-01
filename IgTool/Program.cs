using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using IgTool.IgModel;

namespace IgTool
{                                                                                                  
    class Program
    {
        static void Main(string[] args)
        {
            const string test = "hopsa hopsa od sasa do lasa";

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
            }

            Console.ReadLine();
        }
    }
}
