namespace IgTool.IgModel
{
    /// <summary>
    /// Abstract base for <see cref="Node"/> and <see cref="NodeFix"/>.
    /// </summary>
    public abstract class NodeBase
    {
        /// <summary>
        /// Gets or sets the span's start position.
        /// </summary>
        /// <value>
        /// The text span's start position, relative to parent node.
        /// </value>
        public virtual int? SpanStart { get; set; }

        public virtual int? SpanLength { get; set; }

        public virtual string Text { get; set; }
    }
}
