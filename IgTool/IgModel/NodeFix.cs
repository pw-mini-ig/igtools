using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace IgTool.IgModel
{
    /// <summary>
    /// Represents a set of mutations that should be applied to a node to make it valid.
    /// Properties of this object will be used to change the values of corresponding properties
    /// </summary>
    public class NodeFix : NodeBase
    {
        public Node TargetNode { get; }

        public NodeFix(Node targetNode)
        {
            TargetNode = targetNode;
        }

        /// <summary>
        /// Applies the mutations to <see cref="TargetNode"/>.
        /// </summary>
        public void Apply()
        {
            if (SpanStart != null)
                TargetNode.SpanStart = SpanStart;
            if (SpanLength != null)
                TargetNode.SpanLength = SpanLength;
            if (Text != null)
                TargetNode.Text = Text;
        }

        public string ToString(int indent)
        {
            var sb = new StringBuilder();
            sb.Indent(indent).AppendLine("Node fix:");
            if (SpanStart != null)
                sb.Indent(indent + 1).Append("SpanStart = ").AppendLine(SpanStart.ToString());
            if (SpanLength != null)
                sb.Indent(indent + 1).Append("SpanLength = ").AppendLine(SpanLength.ToString());
            if (Text != null)
                sb.Indent(indent + 1).Append("Text = ").AppendLine(Text);

            if (SpanStart == null && SpanLength == null && Text == null)
                sb.Indent(indent + 1).AppendLine("(empty)");

            return sb.ToString();
        }

        public override string ToString() => ToString(0);
    }
}
