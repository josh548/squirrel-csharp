using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using squirrel.Nodes;

namespace squirrel
{
    public class DotGrapher
    {
        private readonly INode _root;
        private readonly StringBuilder _sb = new StringBuilder();
        private readonly List<INode> _nodes = new List<INode>();

        private int LookUp(INode node)
        {
            for (var i = 0; i < _nodes.Count; i++)
            {
                if (ReferenceEquals(node, _nodes[i]))
                {
                    return i + 1;
                }
            }
            throw new KeyNotFoundException();
        }

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
            _nodes.Add(node);

            var methodName = $"Visit{node.GetType().Name}";
            var method = GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
            method.Invoke(this, new object[] {node});
        }

        private void VisitIntegerNode(IntegerNode node)
        {
            _sb.Append($"\tnode{_nodes.Count} [label=\"Integer({node.Value})\"]\n");
        }

        private void VisitSymbolNode(SymbolNode node)
        {
            _sb.Append($"\tnode{_nodes.Count} [label=\"Symbol({node.Value})\"]\n");
        }

        private void VisitSymbolicExpressionNode(SymbolicExpressionNode node)
        {
            _sb.Append($"\tnode{_nodes.Count} [label=\"SymbolicExpression\"]\n");

            foreach (var child in node.Children)
            {
                VisitNode(child);
            }

            foreach (var child in node.Children)
            {
                _sb.Append($"\tnode{LookUp(node)} -> node{LookUp(child)}\n");
            }
        }

        private void VisitQuotedExpressionNode(QuotedExpressionNode node)
        {
            _sb.Append($"\tnode{_nodes.Count} [label=\"QuotedExpression\"]\n");

            foreach (var child in node.Children)
            {
                VisitNode(child);
            }

            foreach (var child in node.Children)
            {
                _sb.Append($"\tnode{LookUp(node)} -> node{LookUp(child)}\n");
            }
        }
    }
}
