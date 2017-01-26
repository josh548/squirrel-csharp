using System.Collections.Generic;
using System.Linq;

namespace Squirrel.Nodes
{
    public class SymbolicExpressionNode : INode
    {
        public readonly List<INode> Children;

        public SymbolicExpressionNode(List<INode> children)
        {
            Children = children;
        }

        protected bool Equals(SymbolicExpressionNode other)
        {
            return Children.SequenceEqual(other.Children);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((SymbolicExpressionNode)obj);
        }

        public override int GetHashCode()
        {
            return Children?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return $"({string.Join(" ", Children)})";
        }
    }
}
