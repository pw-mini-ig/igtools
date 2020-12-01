using System.Text;

namespace IgTool.IgModel
{
    public class Tree
    {
        public string Text { get; }
        public Node Root { get; }

        public Tree(string text, Node root)
        {
            Text = text;
            Root = root;
        }

        public TreeInspectionStatus InspectTree()
        {
            if (Root == null) return new TreeInspectionStatus(true);
            return Root.InspectRecursive(true);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            AppendToStringBuilderRecursive(sb, Root, 0);

            return sb.ToString();
        }

        private void AppendToStringBuilderRecursive(StringBuilder sb, Node node, int level)
        {
            for (int i = 0; i < level; i++)
                sb.Append("  ");

            if (node == null)
            {
                sb.AppendLine("NULL");
                return;
            }

            sb.AppendLine(node.ToString());
            foreach (var nodeChild in node.Children)
                AppendToStringBuilderRecursive(sb, nodeChild, level + 1);
        }
    }
}
