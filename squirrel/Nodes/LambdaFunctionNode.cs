namespace squirrel.Nodes
{
    public class LambdaFunctionNode : INode
    {
        public readonly QuotedExpressionNode Parameters, Body;

        public LambdaFunctionNode(QuotedExpressionNode parameters, QuotedExpressionNode body)
        {
            Parameters = parameters;
            Body = body;
        }

        protected bool Equals(LambdaFunctionNode other)
        {
            return Equals(Parameters, other.Parameters) && Equals(Body, other.Body);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((LambdaFunctionNode) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Parameters?.GetHashCode() ?? 0) * 397) ^ (Body?.GetHashCode() ?? 0);
            }
        }

        public override string ToString()
        {
            return $"(lambda {Parameters} {Body})";
        }
    }
}
