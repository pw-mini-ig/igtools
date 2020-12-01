using System.Collections.Generic;
using System.Text;

namespace IgTool.IgModel
{
    public enum TreeInspectionProblemType
    {
        InvalidStructure,
        NoTextInNode,
        TextNotPresentInParent,
        InvalidTextLength,
        InvalidTextSpanPosition
    }

    public enum TreeInspectionAutofixStatus
    {
        Impossible,
        Ambiguous,
        Possible
    }

    public class TreeInspectionStatus
    {
        public bool IsOk { get; }
        public Node ProblematicNode { get; }

        public TreeInspectionProblemType? ProblemType { get; }

        public TreeInspectionAutofixStatus? AutofixStatus
        {
            get
            {
                if (IsOk) return null;
                if (PossibleFixes.Count > 1) return TreeInspectionAutofixStatus.Ambiguous;
                if (PossibleFixes.Count == 1) return TreeInspectionAutofixStatus.Possible;
                return TreeInspectionAutofixStatus.Impossible;
            }
        }

        public List<NodeFix> PossibleFixes { get; }

        public TreeInspectionStatus(bool isOk, Node problematicNode = null, 
            TreeInspectionProblemType? problemType = null)
        {
            IsOk = isOk;
            ProblematicNode = problematicNode;
            ProblemType = problemType;
            PossibleFixes = new List<NodeFix>();
        }

        public void AddNodeFix(NodeFix fix) => PossibleFixes.Add(fix);

        public void AddNodeFixRange(IEnumerable<NodeFix> fixes)
        {
            foreach (var nodeFix in fixes)
                AddNodeFix(nodeFix);
        }

        public override string ToString()
        {
            var sb = new StringBuilder("Tree inspection results:").AppendLine();
            if (IsOk)
            {
                sb.Indent().AppendLine("Everything is OK.");
                return sb.ToString();
            }

            sb.Indent().Append("Problem: ").Append(ProblemType).AppendLine();
            sb.Indent().Append("With node: ").AppendLine(ProblematicNode?.ToString() ?? "MISSING");
            sb.Indent().Append("An automatic fix is: ").Append(AutofixStatus).AppendLine();

            if (AutofixStatus == TreeInspectionAutofixStatus.Impossible)
                return sb.ToString();

            sb.Indent().AppendLine("Possible fixes:");
            foreach (var possibleFix in PossibleFixes)
                sb.Append(possibleFix.ToString(2));

            return sb.ToString();
        }
    }
}
