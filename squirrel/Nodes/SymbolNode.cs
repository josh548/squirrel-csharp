namespace squirrel.Nodes
{
    public class SymbolNode : INode
    {
        public readonly string Value;

        public SymbolNode(string value)
        {
            Value = value;
        }

        protected bool Equals(SymbolNode other)
        {
            return string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((SymbolNode) obj);
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
