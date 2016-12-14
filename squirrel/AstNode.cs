using System;
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

        public override string ToString()
        {
            switch (Type)
            {
                case NodeType.Integer:
                case NodeType.Symbol:
                    return Value.ToString();
                case NodeType.SymbolicExpression:
                    return $"({string.Join(" ", Children)})";
                case NodeType.QuotedExpression:
                    return $"{{{string.Join(" ", Children)}}}";
                case NodeType.Lambda:
                case NodeType.Error:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
