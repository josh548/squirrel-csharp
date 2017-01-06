using System.Collections.Generic;
using System.Linq;

namespace Squirrel.Nodes
{
    public class QuotedExpressionNode : INode
    {
        public readonly List<INode> Children;

        public QuotedExpressionNode(List<INode> children)
        {
            Children = children;
        }

        protected bool Equals(QuotedExpressionNode other)
        {
            return Children.SequenceEqual(other.Children);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((QuotedExpressionNode) obj);
        }

        public override int GetHashCode()
        {
            return Children?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return $"{{{string.Join(" ", Children)}}}";
        }
    }
}
