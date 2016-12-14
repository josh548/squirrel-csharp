using System.Collections.Generic;
using System.Text;

namespace squirrel
{
    public class DotGrapher : Visitor
    {
        private readonly AstNode _root;
        private readonly StringBuilder _sb = new StringBuilder();
        private readonly Dictionary<AstNode, int> _nodeNums = new Dictionary<AstNode, int>();
        private int _nodeCount;

        public DotGrapher(AstNode root)
        {
            _root = root;
        }

        public string GenerateGraph()
        {
            _sb.Append("digraph astgraph {\n");
            _sb.Append("\tnode [shape=circle, fontname=\"Courier\"];\n");
            Visit(_root);
            _sb.Append("}");

            return _sb.ToString();
        }

        // ReSharper disable once UnusedMember.Local
        private void VisitInteger(AstNode node)
        {
            _nodeNums.Add(node, ++_nodeCount);

            _sb.Append($"\tnode{_nodeCount} [label=\"Integer({node.Value})\"]\n");
        }

        // ReSharper disable once UnusedMember.Local
        private void VisitWord(AstNode node)
        {
            _nodeNums.Add(node, ++_nodeCount);

            _sb.Append($"\tnode{_nodeCount} [label=\"Word({node.Value})\"]\n");
        }

        // ReSharper disable once UnusedMember.Local
        private void VisitSymbolicExpression(AstNode node)
        {
            _nodeNums.Add(node, ++_nodeCount);

            _sb.Append($"\tnode{_nodeCount} [label=\"SymbolicExpression\"]\n");

            foreach (var child in node.Children)
            {
                Visit(child);
            }

            foreach (var child in node.Children)
            {
                _sb.Append($"\tnode{_nodeNums[node]} -> node{_nodeNums[child]}\n");
            }
        }

        // ReSharper disable once UnusedMember.Local
        private void VisitQuotedExpression(AstNode node)
        {
            _nodeNums.Add(node, ++_nodeCount);

            _sb.Append($"\tnode{_nodeCount} [label=\"QuotedExpression\"]\n");

            foreach (var child in node.Children)
            {
                Visit(child);
            }

            foreach (var child in node.Children)
            {
                _sb.Append($"\tnode{_nodeNums[node]} -> node{_nodeNums[child]}\n");
            }
        }
    }
}
