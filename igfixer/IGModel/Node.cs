using System.Collections.Generic;
using System.Linq;

namespace IGFixer.IGModel
{
    /// <summary>
    /// Node in an IG tree.
    /// </summary>
    /// <seealso cref="IGFixer.IGModel.NodeBase" />
    class Node : NodeBase
    {
        public override int? SpanStart
        {
            get => _spanStart;
            set
            {
                _spanStart = value;
                // Force update to lazily-evaluated property
                _absoluteSpanStart = null;
            }
        }

        public int? AbsoluteSpanStart
        {
            get
            {
                if (_spanStart == null) return null;
                if (_absoluteSpanStart != null) return _absoluteSpanStart;

                if (Parent == null) _absoluteSpanStart = _spanStart;
                else if (Parent.AbsoluteSpanStart == null) return null;
                else _absoluteSpanStart = Parent.AbsoluteSpanStart + _spanStart;

                return _absoluteSpanStart;
            }
        }

        public List<Node> Children { get; }

        public Node Parent { get; }

        private int? _absoluteSpanStart;
        private int? _spanStart;

        public Node(Node parent = null)
        {
            Parent = parent;
            Children = new List<Node>();

            Parent?.AddChild(this);
        }

        /// <summary>
        /// Adds a child node.
        /// </summary>
        /// <param name="node">The child to be added.</param>
        /// <returns>true on success, false otherwise</returns>
        public bool AddChild(Node node)
        {
            if (Children.Contains(node)) return false;
            Children.Add(node);
            return true;
        }

        /// <summary>
        /// Recursively inspects this node and its children. Assumes the parent was previously checked and is OK.
        /// </summary>
        /// <param name="isRoot">whether this node is the root of the tree</param>
        /// <returns></returns>
        public TreeInspectionStatus InspectRecursive(bool isRoot = false)
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                return new TreeInspectionStatus(false, this,
                    TreeInspectionProblemType.NoTextInNode);
            }

            if (Text.Length != SpanLength)
            {
                var status = new TreeInspectionStatus(false, this,
                    TreeInspectionProblemType.InvalidTextLength);
                status.AddNodeFix(new NodeFix(this) { SpanLength = Text.Length });
                return status;
            }
                

            if (!isRoot)
            {
                if (Parent == null)
                    return new TreeInspectionStatus(false, this,
                        TreeInspectionProblemType.InvalidStructure);

                var pText = Parent.Text ?? "";
                var parentOccurences = pText.AllIndexesOf(Text).ToArray();
                if (parentOccurences.Length == 0)
                    return new TreeInspectionStatus(false, this,
                        TreeInspectionProblemType.TextNotPresentInParent);

                if (!parentOccurences.Contains(SpanStart ?? -1))
                {
                    var status = new TreeInspectionStatus(false, this,
                        TreeInspectionProblemType.InvalidTextSpanPosition);
                    var fixes = parentOccurences.Select(x => new NodeFix(this) { SpanStart = x });
                    status.AddNodeFixRange(fixes);
                    return status;
                }
            }
            else if (Parent != null)
            {
                return new TreeInspectionStatus(false, this,
                    TreeInspectionProblemType.InvalidStructure);
            }

            foreach (var child in Children)
            {
                var status = child.InspectRecursive();
                if (!status.IsOk) return status;
            }

            return new TreeInspectionStatus(true);
        }

        public override string ToString()
        {
            return $"Node: {{{Text ?? ""}}}, start: {SpanStart?.ToString() ?? "MISSING"}, " +
                   $"length: {SpanLength?.ToString() ?? "MISSING"}";
        }
    }
}
