using System.Collections.Generic;

namespace squirrel.Nodes
{
    public class QuotedExpressionNode : INode
    {
        public readonly List<INode> Children;

        public QuotedExpressionNode(List<INode> children)
        {
            Children = children;
        }

        public override string ToString()
        {
            return $"{{{string.Join(" ", Children)}}}";
        }
    }
}
