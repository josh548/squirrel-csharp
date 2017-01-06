using Xunit;
using Squirrel;

namespace Tests
{
    public class Tests
    {
        [Fact]
        public void Test()
        {
            var input = "(add 1 2)";
            var expected = "3";

            var tokenizer = new Tokenizer(input);
            var parser = new Parser(tokenizer.Tokenize());
            var evaluator = new Evaluator(parser.Parse());
            var output = evaluator.Evaluate().ToString();
            Assert.Equal(expected, output);
        }
    }
}
