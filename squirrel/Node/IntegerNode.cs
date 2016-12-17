namespace squirrel.Node
{
    public class IntegerNode : INode
    {
        public readonly int Value;

        public IntegerNode(int value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
