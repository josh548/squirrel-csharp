using System;

namespace squirrel
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class BuiltinFunctionAttribute : Attribute
    {
        public NodeType[] ExpectedTypes;
    }
}
