using System;
using System.Collections.Generic;
using System.Reflection;

namespace squirrel
{
    public class Evaluator : Visitor
    {
        private readonly AstNode _root;

        public Evaluator(AstNode root)
        {
            _root = root;
        }

        public AstNode Evaluate()
        {
            return Visit(_root, new Environment());
        }

        protected AstNode VisitInteger(AstNode node, Environment env)
        {
            return node;
        }

        protected AstNode VisitSymbol(AstNode node, Environment env)
        {
            return node;
        }

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

        protected AstNode VisitQuotedExpression(AstNode node, Environment env)
        {
            return node;
        }

        private AstNode EvaluateBuiltinFunction(AstNode head, List<AstNode> tail, Environment env)
        {
            var methodName = $"Builtin{head.Value.ToString().Capitalize()}";
            var method = GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
            return (AstNode) method.Invoke(this, new object[] {tail, env});
        }
    }
}
