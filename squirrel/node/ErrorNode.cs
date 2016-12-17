namespace squirrel.Node
{
    public class ErrorNode : INode
    {
        public readonly string Message;

        public ErrorNode(string message)
        {
            Message = message;
        }

        public override string ToString()
        {
            return Message;
        }
    }
}
