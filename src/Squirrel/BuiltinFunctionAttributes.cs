using System;

namespace Squirrel
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class ExpectedTypeAttribute : Attribute
    {
        public readonly Type ExpectedType;

        public ExpectedTypeAttribute(Type expectedType)
        {
            ExpectedType = expectedType;
        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class ExpectedTypesAttribute : Attribute
    {
        public readonly Type[] ExpectedTypes;

        public ExpectedTypesAttribute(params Type[] expectedTypes)
        {
            ExpectedTypes = expectedTypes;
        }
    }
}
