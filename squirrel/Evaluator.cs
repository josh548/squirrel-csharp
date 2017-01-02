using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Squirrel.Exceptions;
using Squirrel.Nodes;

namespace Squirrel
{
    public class Evaluator
    {
        private readonly INode _root;

        private static readonly Dictionary<string, BuiltinFunctionDelegate> BuiltinFunctions =
            new Dictionary<string, BuiltinFunctionDelegate>
            {
                {"display", BuiltinDisplay},
                {"block", BuiltinBlock},
                {"def", BuiltinDef},
                {"outer", BuiltinOuter},
                {"add", BuiltinAdd},
                {"sub", BuiltinSub},
                {"mul", BuiltinMul},
                {"div", BuiltinDiv},
                {"mod", BuiltinMod},
                {"eval", BuiltinEval},
                {"quote", BuiltinQuote},
                {"lambda", BuiltinLambda},
                {"eq", BuiltinEq},
                {"lt", BuiltinLt},
                {"gt", BuiltinGt},
                {"nth", BuiltinNth},
                {"len", BuiltinLen},
                {"head", BuiltinHead},
                {"tail", BuiltinTail},
                {"join", BuiltinJoin},
                {"when", BuiltinWhen}
            };

        private delegate INode BuiltinFunctionDelegate(List<INode> args, Environment env);

        private static readonly INode True = new SymbolNode("true");
        private static readonly INode False = new SymbolNode("false");
        private static readonly INode Nil = new QuotedExpressionNode(new List<INode>());

        public Evaluator(INode root)
        {
            _root = root;
        }

        public INode Evaluate() => VisitNode(_root, new Environment());

        private static INode VisitNode(INode node, Environment env)
        {
            var methodName = $"Visit{node.GetType().Name}";
            var method = typeof(Evaluator).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
            return (INode) method.Invoke(null, new object[] {node, env});
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        private static INode VisitIntegerNode(IntegerNode node, Environment env) => node;

        // ReSharper disable once UnusedMember.Local
        private static INode VisitSymbolNode(SymbolNode node, Environment env)
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

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        private static INode VisitStringNode(StringNode node, Environment env) => node;

        // ReSharper disable once UnusedMember.Local
        private static INode VisitSymbolicExpressionNode(SymbolicExpressionNode node, Environment env)
        {
            if (node.Children.Count < 2)
            {
                throw new EvaluatorException("symbolic expression must contain a symbol and at least one argument");
            }

            var visitedChildren = new List<INode>();
            foreach (var child in node.Children)
            {
                var visitedChild = VisitNode(child, env);
                if (visitedChild.GetType() == typeof(ErrorNode))
                {
                    return visitedChild;
                }
                visitedChildren.Add(visitedChild);
            }

            var head = visitedChildren.Head();
            var tail = visitedChildren.Tail();

            if (head.GetType() == typeof(SymbolNode))
            {
                return EvaluateBuiltinFunction((SymbolNode) head, tail, env);
            }

            if (head.GetType() == typeof(LambdaFunctionNode))
            {
                return EvaluateLambdaFunction((LambdaFunctionNode) head, tail, new Environment(env));
            }

            throw new EvaluatorException("first element of symbolic expression must be a symbol or lambda function");
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        private static INode VisitQuotedExpressionNode(QuotedExpressionNode node, Environment env) => node;

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        private static INode VisitLambdaFunctionNode(LambdaFunctionNode node, Environment env) => node;

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        private static INode VisitErrorNode(ErrorNode node, Environment env) => node;

        private static INode EvaluateBuiltinFunction(SymbolNode head, List<INode> tail, Environment env)
        {
            var functionName = head.Value;

            if (!BuiltinFunctions.ContainsKey(functionName))
            {
                throw new EvaluatorException($"function is not defined: {functionName}");
            }

            var function = BuiltinFunctions[functionName];

            var expectedTypeAttr = function.GetMethodInfo().GetCustomAttribute<ExpectedTypeAttribute>();

            if (expectedTypeAttr != null)
            {
                var expectedType = expectedTypeAttr.ExpectedType;
                foreach (var arg in tail)
                {
                    var actualType = arg.GetType();
                    if (actualType != expectedType)
                    {
                        return new ErrorNode($"expected argument of type {expectedType} but got type {actualType}");
                    }
                }
            }

            var expectedTypesAttr = function.GetMethodInfo().GetCustomAttribute<ExpectedTypesAttribute>();

            if (expectedTypesAttr != null)
            {
                var expectedTypes = expectedTypesAttr.ExpectedTypes;

                var expectedCount = expectedTypes.Length;
                var actualCount = tail.Count;

                if (actualCount != expectedCount)
                {
                    return new ErrorNode($"function takes exactly {expectedCount} arguments ({actualCount} given)");
                }

                for (var i = 0; i < expectedCount; i++)
                {
                    var expectedType = expectedTypes[i];
                    var actualType = tail[i].GetType();

                    if (actualType != expectedType)
                    {
                        return new ErrorNode($"expected argument of type {expectedType} but got type {actualType}");
                    }
                }
            }

            return function.Invoke(tail, env);
        }

        private static INode EvaluateLambdaFunction(LambdaFunctionNode head, List<INode> tail, Environment env)
        {
            var expectedCount = head.Parameters.Children.Count;
            var actualCount = tail.Count;

            if (actualCount != expectedCount)
            {
                return new ErrorNode($"function takes exactly {expectedCount} arguments ({actualCount} given)");
            }

            for (var i = 0; i < expectedCount; i++)
            {
                var key = ((SymbolNode) head.Parameters.Children[i]).Value;
                var value = tail[i];
                env.Put(key, value);
            }

            return BuiltinEval(new List<INode> {head.Body}, env);
        }

        private static INode BuiltinDisplay(List<INode> args, Environment env)
        {
            Console.WriteLine(args[0]);
            return Nil;
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

            return Nil;
        }

        private static INode BuiltinOuter(List<INode> args, Environment env)
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

                env.PutOuter(name, value);
            }

            return Nil;
        }

        [ExpectedType(typeof(IntegerNode))]
        private static INode BuiltinAdd(List<INode> args, Environment env)
        {
            if (args.Count < 2)
            {
                return new ErrorNode($"function takes at least 2 arguments ({args.Count} given)");
            }
            var sum = args.Sum(arg => ((IntegerNode) arg).Value);
            return new IntegerNode(sum);
        }

        [ExpectedTypes(new[] {typeof(IntegerNode), typeof(IntegerNode)})]
        private static INode BuiltinSub(List<INode> args, Environment env)
        {
            var first = ((IntegerNode) args[0]).Value;
            var second = ((IntegerNode) args[1]).Value;
            var difference = first - second;
            return new IntegerNode(difference);
        }

        [ExpectedType(typeof(IntegerNode))]
        private static INode BuiltinMul(List<INode> args, Environment env)
        {
            if (args.Count < 2)
            {
                return new ErrorNode($"function takes exactly 2 arguments ({args.Count} given)");
            }
            var product = args.Aggregate(1, (current, arg) => current * ((IntegerNode) arg).Value);
            return new IntegerNode(product);
        }

        [ExpectedTypes(new[] {typeof(IntegerNode), typeof(IntegerNode)})]
        private static INode BuiltinDiv(List<INode> args, Environment env)
        {
            var first = ((IntegerNode) args[0]).Value;
            var second = ((IntegerNode) args[1]).Value;

            if (second == 0)
            {
                return new ErrorNode("cannot divide by zero");
            }

            var quotient = first / second;
            return new IntegerNode(quotient);
        }

        [ExpectedTypes(new[] {typeof(IntegerNode), typeof(IntegerNode)})]
        private static INode BuiltinMod(List<INode> args, Environment env)
        {
            var first = ((IntegerNode) args[0]).Value;
            var second = ((IntegerNode) args[1]).Value;

            if (second == 0)
            {
                return new ErrorNode("cannot divide by zero");
            }

            var remainder = first % second;
            return new IntegerNode(remainder);
        }

        [ExpectedTypes(new[] {typeof(QuotedExpressionNode)})]
        private static INode BuiltinEval(List<INode> args, Environment env)
        {
            var children = ((QuotedExpressionNode) args[0]).Children;

            switch (children.Count)
            {
                case 0:
                    return new ErrorNode("cannot evalute empty quoted expression");
                case 1:
                    return VisitNode(children[0], env);
                default:
                    return VisitNode(new SymbolicExpressionNode(children), env);
            }
        }

        private static INode BuiltinQuote(List<INode> args, Environment env)
        {
            return new QuotedExpressionNode(args);
        }

        [ExpectedTypes(new[] {typeof(QuotedExpressionNode), typeof(QuotedExpressionNode)})]
        private static INode BuiltinLambda(List<INode> args, Environment env)
        {
            var parameters = (QuotedExpressionNode) args[0];
            if (parameters.Children.Any(node => !(node is SymbolNode)))
            {
                return new ErrorNode("list of lambda function parameters must contain only symbols");
            }

            var body = (QuotedExpressionNode) args[1];

            return new LambdaFunctionNode(parameters, body);
        }

        private static INode BuiltinEq(List<INode> args, Environment env)
        {
            if (args.Count < 2)
            {
                return new ErrorNode($"function takes exactly 2 arguments ({args.Count} given)");
            }

            return args[0].Equals(args[1]) ? True : False;
        }

        [ExpectedTypes(new[] {typeof(IntegerNode), typeof(IntegerNode)})]
        private static INode BuiltinLt(List<INode> args, Environment env)
        {
            var first = ((IntegerNode) args[0]).Value;
            var second = ((IntegerNode) args[1]).Value;
            return first < second ? True : False;
        }

        [ExpectedTypes(new[] {typeof(IntegerNode), typeof(IntegerNode)})]
        private static INode BuiltinGt(List<INode> args, Environment env)
        {
            var first = ((IntegerNode) args[0]).Value;
            var second = ((IntegerNode) args[1]).Value;
            return first > second ? True : False;
        }

        [ExpectedTypes(new[] {typeof(QuotedExpressionNode), typeof(IntegerNode)})]
        private static INode BuiltinNth(List<INode> args, Environment env)
        {
            var list = ((QuotedExpressionNode) args[0]).Children;
            var n = ((IntegerNode) args[1]).Value;

            if (n < 1)
            {
                return new ErrorNode($"n ({n}) must be greater than 0");
            }

            if (n > list.Count)
            {
                return new ErrorNode($"n ({n}) must not be greater than the length of the list ({list.Count})");
            }

            return list[n - 1];
        }

        [ExpectedTypes(new[] {typeof(QuotedExpressionNode)})]
        private static INode BuiltinLen(List<INode> args, Environment env)
        {
            return new IntegerNode(((QuotedExpressionNode) args[0]).Children.Count);
        }

        [ExpectedTypes(new[] {typeof(QuotedExpressionNode)})]
        private static INode BuiltinHead(List<INode> args, Environment env)
        {
            var list = ((QuotedExpressionNode) args[0]).Children;
            if (list.Count == 0)
            {
                return Nil;
            }
            return VisitNode(list.Head(), env);
        }

        [ExpectedTypes(new[] {typeof(QuotedExpressionNode)})]
        private static INode BuiltinTail(List<INode> args, Environment env)
        {
            var tail = ((QuotedExpressionNode) args[0]).Children.Tail();
            return new QuotedExpressionNode(tail);
        }

        [ExpectedType(typeof(QuotedExpressionNode))]
        private static INode BuiltinJoin(List<INode> args, Environment env)
        {
            var joined = new List<INode>();
            foreach (var arg in args)
            {
                joined.AddRange(((QuotedExpressionNode) arg).Children);
            }
            return new QuotedExpressionNode(joined);
        }

        [ExpectedType(typeof(QuotedExpressionNode))]
        private static INode BuiltinWhen(List<INode> args, Environment env)
        {
            var clauses = args.Cast<QuotedExpressionNode>().ToList();

            if (clauses.Any(clause => clause.Children.Count != 2))
            {
                return new ErrorNode("each clause must consist of a condition and a result");
            }

            foreach (var clause in clauses)
            {
                var condition = clause.Children[0];
                var outcome = VisitNode(condition, env);

                if (outcome.Equals(True))
                {
                    return VisitNode(clause.Children[1], env);
                }

                if (outcome.Equals(False))
                {
                    continue;
                }

                return new ErrorNode("the outcome of a condition must resolve to either true or false");
            }

            return new ErrorNode("no condition was met");
        }
    }
}
