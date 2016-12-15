using System;
using System.Collections.Generic;
using System.Linq;

namespace squirrel
{
    public class Evaluator : Visitor
    {
        private readonly AstNode _root;

        private readonly Dictionary<string, BuiltinFunctionDelegate> _builtinFunctions =
            new Dictionary<string, BuiltinFunctionDelegate>()
            {
                {"add", BuiltinAdd},
                {"sub", BuiltinSub},
                {"mul", BuiltinMul},
                {"div", BuiltinDiv}
            };

        public delegate AstNode BuiltinFunctionDelegate(List<AstNode> args, Environment env);

        public Evaluator(AstNode root)
        {
            _root = root;
        }

        public AstNode Evaluate() => Visit(_root, new Environment());

        protected AstNode VisitInteger(AstNode node, Environment env) => node;

        protected AstNode VisitSymbol(AstNode node, Environment env) => node;

        protected AstNode VisitSymbolicExpression(AstNode node, Environment env)
        {
            if (node.Children.Count < 2)
            {
                throw new ArgumentException("symbolic expression must contain a symbol and at least one argument");
            }

            var visitedChildren = new List<AstNode>();
            foreach (var child in node.Children)
            {
                var visitedChild = Visit(child, env);
                if (visitedChild.Type == NodeType.Error)
                {
                    return visitedChild;
                }
                visitedChildren.Add(visitedChild);
            }

            var head = visitedChildren.Head();

            if (head.Type != NodeType.Symbol)
            {
                throw new ArgumentException("first item in symbolic expression must be a symbol");
            }

            var tail = visitedChildren.Tail();

            return EvaluateBuiltinFunction(head, tail, env);
        }

        protected AstNode VisitQuotedExpression(AstNode node, Environment env) => node;

        protected AstNode VisitError(AstNode node, Environment env) => node;

        private AstNode EvaluateBuiltinFunction(AstNode head, List<AstNode> tail, Environment env)
        {
            var functionName = (string) head.Value;

            if (!_builtinFunctions.ContainsKey(functionName))
            {
                throw new ArgumentException($"function is not defined: {functionName}");
            }

            var function = _builtinFunctions[functionName];
            return function.Invoke(tail, env);
        }

        private static AstNode BuiltinAdd(List<AstNode> args, Environment env)
        {
            if (args.Count < 2)
            {
                return new AstNode(NodeType.Error, null, $"function takes at least 2 arguments ({args.Count} given)");
            }

            if (args.Exists(arg => arg.Type != NodeType.Integer))
            {
                return new AstNode(NodeType.Error, null, $"arguments must be of type {NodeType.Integer}");
            }

            var sum = args.Sum(arg => (int) arg.Value);
            return new AstNode(NodeType.Integer, null, sum);
        }

        private static AstNode BuiltinSub(List<AstNode> args, Environment env)
        {
            if (args.Count != 2)
            {
                return new AstNode(NodeType.Error, null, $"function takes exactly 2 arguments ({args.Count} given)");
            }

            if (args.Exists(arg => arg.Type != NodeType.Integer))
            {
                return new AstNode(NodeType.Error, null, $"arguments must be of type {NodeType.Integer}");
            }

            var first = (int) args[0].Value;
            var second = (int) args[1].Value;
            var difference = first - second;
            return new AstNode(NodeType.Integer, null, difference);
        }

        private static AstNode BuiltinMul(List<AstNode> args, Environment env)
        {
            if (args.Count < 2)
            {
                return new AstNode(NodeType.Error, null, $"function takes exactly 2 arguments ({args.Count} given)");
            }

            if (args.Exists(arg => arg.Type != NodeType.Integer))
            {
                return new AstNode(NodeType.Error, null, $"arguments must be of type {NodeType.Integer}");
            }

            var product = args.Aggregate(1, (current, arg) => current * (int) arg.Value);
            return new AstNode(NodeType.Integer, null, product);
        }

        private static AstNode BuiltinDiv(List<AstNode> args, Environment env)
        {
            if (args.Count != 2)
            {
                return new AstNode(NodeType.Error, null, $"function takes exactly 2 arguments ({args.Count} given)");
            }

            if (args.Exists(arg => arg.Type != NodeType.Integer))
            {
                return new AstNode(NodeType.Error, null, $"arguments must be of type {NodeType.Integer}");
            }

            var first = (int) args[0].Value;
            var second = (int) args[1].Value;

            if (second == 0)
            {
                return new AstNode(NodeType.Error, null, "cannot divide by zero");
            }

            var quotient = first / second;
            return new AstNode(NodeType.Integer, null, quotient);
        }
    }
}
