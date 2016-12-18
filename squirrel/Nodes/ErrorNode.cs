namespace squirrel.Nodes
{
    public class ErrorNode : INode
    {
        public readonly string Message;

        public ErrorNode(string message)
        {
            Message = message;
        }

        protected bool Equals(ErrorNode other)
        {
            return string.Equals(Message, other.Message);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ErrorNode) obj);
        }

        public override int GetHashCode()
        {
            return Message?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return Message;
        }
    }
}
