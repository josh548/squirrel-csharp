namespace squirrel.Node
{
    public class SymbolNode : INode
    {
        public readonly string Value;

        public SymbolNode(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
