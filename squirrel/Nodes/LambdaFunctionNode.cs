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

        public override string ToString()
        {
            return $"(lambda {Parameters} {Body})";
        }
    }
}
