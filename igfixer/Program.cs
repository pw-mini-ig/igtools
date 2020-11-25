using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using IGFixer.IGModel;

namespace IGFixer
{                                                                                                  
    class Program
    {
        static void Main(string[] args)
        {
            var serializer = new XmlSerializer(typeof(Xml.Sentence));
            Xml.Sentence fromFile;
            using (var fs = new FileStream("example.xml", FileMode.Open))
            {
                fromFile = (Xml.Sentence) serializer.Deserialize(fs);
            }

            /*const string test = "hopsa hopsa od sasa do lasa";

            var root = new Node
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
            */

            Console.ReadLine();
        }
    }
}
