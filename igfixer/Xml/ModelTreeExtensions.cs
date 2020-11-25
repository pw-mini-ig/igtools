using System;
using System.Collections.Generic;
using System.Text;
using IGFixer.IGModel;

namespace IGFixer.Xml
{
    public static class ModelTreeExtensions
    {
        public static Tree FromXml(this Tree tree, Sentence sentence)
        {
            /*var root = new Node
            {
                Text = sentence.Content,
                SpanStart = 0,
                SpanLength = sentence.Content.Length
            };*/
            return new Tree("", null);
        }
    }
}
