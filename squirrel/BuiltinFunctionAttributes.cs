﻿using System;
using squirrel.Nodes;

namespace squirrel
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

        public ExpectedTypesAttribute(Type[] expectedTypes)
        {
            ExpectedTypes = expectedTypes;
        }
    }
}