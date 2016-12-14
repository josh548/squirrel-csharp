using System.Reflection;

namespace squirrel
{
    public abstract class Visitor
    {
        protected void Visit(AstNode node)
        {
            var methodName = $"Visit{node.Type}";
            var method = GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
            method.Invoke(this, new object[] {node});
        }
    }
}
