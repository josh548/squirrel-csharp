using Xunit;
using Squirrel;
using Squirrel.Nodes;

namespace Tests
{
    public class EnvironmentTests
    {
        private static readonly string TestVariableName = "x";
        private static readonly INode TestVariableValue = new IntegerNode(1);

        [Fact]
        public void TestCanGetValueFromCurrentEnvironment()
        {
            var environment = new Environment();
            environment.Put(TestVariableName, TestVariableValue);
            Assert.Equal(TestVariableValue, environment.Get(TestVariableName));
        }

        [Fact]
        public void TestCanGetValueFromParentEnvironment()
        {
            var parentEnvironment = new Environment();
            var childEnvironment = new Environment(parentEnvironment);
            parentEnvironment.Put(TestVariableName, TestVariableValue);
            Assert.Equal(TestVariableValue, childEnvironment.Get(TestVariableName));
        }

        [Fact]
        public void TestCanGetValueFromGrandparentEnvironment()
        {
            var grandparentEnvironment = new Environment();
            var parentEnvironment = new Environment(grandparentEnvironment);
            var childEnvironment = new Environment(parentEnvironment);
            grandparentEnvironment.Put(TestVariableName, TestVariableValue);
            Assert.Equal(TestVariableValue, childEnvironment.Get(TestVariableName));
        }

        [Fact]
        public void TestCannotGetValueFromChildEnvironment()
        {
            var parentEnvironment = new Environment();
            var childEnvironment = new Environment(parentEnvironment);
            childEnvironment.Put(TestVariableName, TestVariableValue);
            Assert.NotEqual(TestVariableValue, parentEnvironment.Get(TestVariableName));
        }
    }
}
