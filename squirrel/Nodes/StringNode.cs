using System;

namespace Squirrel.Nodes
{
    public class StringNode : INode
    {
        public readonly string Value;

        public StringNode(string value)
        {
            Value = value;
        }

        protected bool Equals(StringNode other)
        {
            return string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((StringNode) obj);
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return $"\"{Value.Replace("\"", "\\\"")}\"";
        }
    }
}
