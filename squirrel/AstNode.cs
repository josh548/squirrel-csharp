using System.Collections.Generic;

namespace squirrel
{
    public struct AstNode
    {
        public readonly NodeType Type;
        public readonly List<AstNode> Children;
        public readonly object Value;

        public AstNode(NodeType type, List<AstNode> children, object value)
        {
            Type = type;
            Children = children;
            Value = value;
        }
    }
}
