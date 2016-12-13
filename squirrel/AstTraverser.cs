using System;
using System.Reflection;

namespace squirrel
{
    public class AstTraverser
    {
        public void Visit(AstNode node)
        {
            var methodName = $"Visit{node.Type}";
            var method = GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
            method.Invoke(this, new object[] {node});
        }

        // ReSharper disable once UnusedMember.Local
        private void VisitInteger(AstNode node)
        {
            Console.WriteLine(node.Type);
        }

        // ReSharper disable once UnusedMember.Local
        private void VisitWord(AstNode node)
        {
            Console.WriteLine(node.Type);
        }

        // ReSharper disable once UnusedMember.Local
        private void VisitSymbolicExpression(AstNode node)
        {
            Console.WriteLine(node.Type);
            foreach (var child in node.Children)
            {
                Visit(child);
            }
        }

        // ReSharper disable once UnusedMember.Local
        private void VisitQuotedExpression(AstNode node)
        {
            Console.WriteLine(node.Type);
            foreach (var child in node.Children)
            {
                Visit(child);
            }
        }
    }
}
