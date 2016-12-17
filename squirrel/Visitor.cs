using System.Reflection;
using squirrel.Node;

namespace squirrel
{
    public abstract class Visitor
    {
        protected void Visit(INode node)
        {
            var methodName = $"Visit{node.GetType().Name}";
            var method = GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
            method.Invoke(this, new object[] {node});
        }

        protected INode Visit(INode node, Environment env)
        {
            var methodName = $"Visit{node.GetType().Name}";
            var method = GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
            return (INode) method.Invoke(this, new object[] {node, env});
        }
    }
}
