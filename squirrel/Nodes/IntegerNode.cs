namespace Squirrel.Nodes
{
    public class IntegerNode : INode
    {
        public readonly int Value;

        public IntegerNode(int value)
        {
            Value = value;
        }

        protected bool Equals(IntegerNode other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((IntegerNode) obj);
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
