using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using squirrel.Node;

namespace squirrel
{
    public class Evaluator : Visitor
    {
        private readonly INode _root;

        private static readonly Dictionary<string, BuiltinFunctionDelegate> BuiltinFunctions =
            new Dictionary<string, BuiltinFunctionDelegate>()
            {
                {"block", BuiltinBlock},
                {"def", BuiltinDef},
                {"add", BuiltinAdd},
                {"sub", BuiltinSub},
                {"mul", BuiltinMul},
                {"div", BuiltinDiv}
            };

        private delegate INode BuiltinFunctionDelegate(List<INode> args, Environment env);

        public Evaluator(INode root)
        {
            _root = root;
        }

        public INode Evaluate() => Visit(_root, new Environment());

        protected INode VisitIntegerNode(IntegerNode node, Environment env) => node;

        protected INode VisitSymbolNode(SymbolNode node, Environment env)
        {
            while (true)
            {
                var nullable = env.Get(node.Value);

                if (nullable == null)
                {
                    return node;
                }

                if (nullable.GetType() != typeof(SymbolNode))
                {
                    return nullable;
                }

                node = (SymbolNode) nullable;
            }
        }

        protected INode VisitSymbolicExpressionNode(SymbolicExpressionNode node, Environment env)
        {
            if (node.Children.Count < 2)
            {
                throw new ArgumentException("symbolic expression must contain a symbol and at least one argument");
            }

            var visitedChildren = new List<INode>();
            foreach (var child in node.Children)
            {
                var visitedChild = Visit(child, env);
                if (visitedChild.GetType() == typeof(ErrorNode))
                {
                    return visitedChild;
                }
                visitedChildren.Add(visitedChild);
            }

            var head = visitedChildren.Head();

            if (head.GetType() != typeof(SymbolNode))
            {
                throw new ArgumentException("first item in symbolic expression must be a symbol");
            }

            var tail = visitedChildren.Tail();

            return EvaluateBuiltinFunction((SymbolNode) head, tail, env);
        }

        protected INode VisitQuotedExpressionNode(QuotedExpressionNode node, Environment env) => node;

        protected INode VisitErrorNode(ErrorNode node, Environment env) => node;

        private static INode EvaluateBuiltinFunction(SymbolNode head, List<INode> tail, Environment env)
        {
            var functionName = head.Value;

            if (!BuiltinFunctions.ContainsKey(functionName))
            {
                throw new ArgumentException($"function is not defined: {functionName}");
            }

            var function = BuiltinFunctions[functionName];
            var attr = function.GetMethodInfo().GetCustomAttribute<BuiltinFunctionAttribute>();

            if (attr == null)
            {
                return function.Invoke(tail, env);
            }

            var expectedCount = attr.ExpectedTypes.Length;
            var actualCount = tail.Count;

            if (actualCount != expectedCount)
            {
                return new ErrorNode($"function takes exactly {expectedCount} arguments ({actualCount} given)");
            }

            for (var i = 0; i < expectedCount; i++)
            {
                var expectedType = attr.ExpectedTypes[i];
                var actualType = tail[i].GetType();

                if (actualType != expectedType)
                {
                    return new ErrorNode($"expected argument of type {expectedType} but got type {actualType}");
                }
            }

            return function.Invoke(tail, env);
        }

        private static INode BuiltinBlock(List<INode> args, Environment env) => args[args.Count - 1];

        private static INode BuiltinDef(List<INode> args, Environment env)
        {
            var names = ((QuotedExpressionNode) (args.Head())).Children;

            if (names.Any(name => name.GetType() != typeof(SymbolNode)))
            {
                return new ErrorNode($"names must be of type {nameof(SymbolNode)}");
            }

            var values = args.Tail();

            if (names.Count != values.Count)
            {
                return new ErrorNode(
                    $"number of values ({values.Count}) must equal number of names ({names.Count})");
            }

            for (var i = 0; i < names.Count; i++)
            {
                var name = ((SymbolNode) names[i]).Value;
                var value = values[i];

                if (BuiltinFunctions.ContainsKey(name))
                {
                    return new ErrorNode($"cannot redefine builtin function: {name}");
                }

                env.Put(name, value);
            }

            return new QuotedExpressionNode(new List<INode>());
        }

        private static INode BuiltinAdd(List<INode> args, Environment env)
        {
            if (args.Count < 2)
            {
                return new ErrorNode($"function takes at least 2 arguments ({args.Count} given)");
            }

            if (args.Exists(arg => arg.GetType() != typeof(IntegerNode)))
            {
                return new ErrorNode($"arguments must be of type {nameof(IntegerNode)}");
            }

            var sum = args.Sum(arg => ((IntegerNode) arg).Value);
            return new IntegerNode(sum);
        }

        [BuiltinFunction(ExpectedTypes = new[] {typeof(IntegerNode), typeof(IntegerNode)})]
        private static INode BuiltinSub(List<INode> args, Environment env)
        {
            var first = ((IntegerNode) args[0]).Value;
            var second = ((IntegerNode) args[1]).Value;
            var difference = first - second;
            return new IntegerNode(difference);
        }

        private static INode BuiltinMul(List<INode> args, Environment env)
        {
            if (args.Count < 2)
            {
                return new ErrorNode($"function takes exactly 2 arguments ({args.Count} given)");
            }

            if (args.Exists(arg => arg.GetType() != typeof(IntegerNode)))
            {
                return new ErrorNode($"arguments must be of type {nameof(IntegerNode)}");
            }

            var product = args.Aggregate(1, (current, arg) => current * ((IntegerNode) arg).Value);
            return new IntegerNode(product);
        }

        [BuiltinFunction(ExpectedTypes = new[] {typeof(IntegerNode), typeof(IntegerNode)})]
        private static INode BuiltinDiv(List<INode> args, Environment env)
        {
            var first = ((IntegerNode) args[0]).Value;
            var second = ((IntegerNode) args[1]).Value;

            if (second == 0)
            {
                return new ErrorNode("cannot divide by zeor");
            }

            var quotient = first / second;
            return new IntegerNode(quotient);
        }
    }
}
