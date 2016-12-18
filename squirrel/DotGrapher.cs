using System.Collections.Generic;
using System.Reflection;
using System.Text;
using squirrel.Nodes;

namespace squirrel
{
    public class DotGrapher
    {
        private readonly INode _root;
        private readonly StringBuilder _sb = new StringBuilder();
        private readonly Dictionary<INode, int> _nodeNums = new Dictionary<INode, int>();
        private int _nodeIndex;

        public DotGrapher(INode root)
        {
            _root = root;
        }

        public string GenerateGraph()
        {
            _sb.Append("digraph astgraph {\n");
            _sb.Append("\tnode [shape=circle, fontname=\"Courier\"];\n");
            VisitNode(_root);
            _sb.Append("}");

            return _sb.ToString();
        }

        private void VisitNode(INode node)
        {
            var methodName = $"Visit{node.GetType().Name}";
            var method = GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
            method.Invoke(this, new object[] {node});
        }

        private void VisitIntegerNode(IntegerNode node)
        {
            _nodeNums.Add(node, _nodeIndex);

            _sb.Append($"\tnode{_nodeIndex++} [label=\"Integer({node.Value})\"]\n");
        }

        private void VisitSymbolNode(SymbolNode node)
        {
            _nodeNums.Add(node, _nodeIndex);

            _sb.Append($"\tnode{_nodeIndex++} [label=\"Symbol({node.Value})\"]\n");
        }

        private void VisitSymbolicExpressionNode(SymbolicExpressionNode node)
        {
            _nodeNums.Add(node, _nodeIndex);

            _sb.Append($"\tnode{_nodeIndex++} [label=\"SymbolicExpression\"]\n");

            foreach (var child in node.Children)
            {
                VisitNode(child);
            }

            foreach (var child in node.Children)
            {
                _sb.Append($"\tnode{_nodeNums[node]} -> node{_nodeNums[child]}\n");
            }
        }

        private void VisitQuotedExpressionNode(QuotedExpressionNode node)
        {
            _nodeNums.Add(node, _nodeIndex);

            _sb.Append($"\tnode{_nodeIndex++} [label=\"QuotedExpression\"]\n");

            foreach (var child in node.Children)
            {
                VisitNode(child);
            }

            foreach (var child in node.Children)
            {
                _sb.Append($"\tnode{_nodeNums[node]} -> node{_nodeNums[child]}\n");
            }
        }
    }
}
