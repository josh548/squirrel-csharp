using System.Reflection;

namespace squirrel
{
    public abstract class Visitor
    {
        protected void Visit(AstNode node)
        {
            var method = GetType().GetMethod($"Visit{node.Type}", BindingFlags.Instance | BindingFlags.NonPublic);
            method.Invoke(this, new object[] {node});
        }

        protected AstNode Visit(AstNode node, Environment env)
        {
            var method = GetType().GetMethod($"Visit{node.Type}", BindingFlags.Instance | BindingFlags.NonPublic);
            return (AstNode) method.Invoke(this, new object[] {node, env});
        }
    }
}
