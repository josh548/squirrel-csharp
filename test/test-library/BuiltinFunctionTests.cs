using Xunit;
using Squirrel;
using Squirrel.Nodes;

namespace Tests
{
    public class BuiltinFunctionTests
    {
        [Theory]
        [InlineData("add", "(add 1 2)", "3")]
        [InlineData("block", "(block (add 1 2))", "3")]
        [InlineData("def", "(def {x} 1)", "null")]
        [InlineData("def", "(block (def {x y} 1 2) (add x y))", "3")]
        [InlineData("display", "(display \"hello world\")", "null")]
        [InlineData("div", "(div 6 3)", "2")]
        [InlineData("eq", "(eq (add 1 2) 3)", "true")]
        [InlineData("gt", "(gt -1 0)", "false")]
        [InlineData("gt", "(gt  0 0)", "false")]
        [InlineData("gt", "(gt +1 0)", "true")]
        [InlineData("id", "(id x)", "x")]
        [InlineData("if", "(if (lt -1 0) {id negative} {id non-negative})", "negative")]
        [InlineData("if", "(if (lt +1 0) {id negative} {id non-negative})", "non-negative")]
        [InlineData("join", "(join {} {})", "{}")]
        [InlineData("join", "(join {a} {b})", "{a b}")]
        [InlineData("lambda", "((lambda {x} {mul x x}) 3)", "9")]
        [InlineData("len", "(len {})", "0")]
        [InlineData("len", "(len {a b c})", "3")]
        [InlineData("lt", "(lt -1 0)", "true")]
        [InlineData("lt", "(lt  0 0)", "false")]
        [InlineData("lt", "(lt +1 0)", "false")]
        [InlineData("mod", "(mod +5 +3)", "2")]
        [InlineData("mod", "(mod +5 -3)", "2")]
        [InlineData("mod", "(mod -5 +3)", "-2")]
        [InlineData("mod", "(mod -5 -3)", "-2")]
        [InlineData("mul", "(mul 2 3)", "6")]
        [InlineData("nth", "(nth {a b c} 2)", "b")]
        [InlineData("outer", "(block (block (outer {x y} 1 2)) (add x y))", "3")]
        [InlineData("print", "(print \"hello world\\n\")", "null")]
        [InlineData("quote", "(quote a b c)", "{a b c}")]
        [InlineData("set", "(block (def {values} {1 two 3}) (set {values} 1 2) values)", "{1 2 3}")]
        [InlineData("slice", "(slice {a b c} 0 1)", "{a}")]
        [InlineData("slice", "(slice {a b c} 1 2)", "{b}")]
        [InlineData("slice", "(slice {a b c} 2 3)", "{c}")]
        [InlineData("slice", "(block (def {letters} {a b c}) (slice letters 0 (len letters)))", "{a b c}")]
        [InlineData("sub", "(sub 3 2)", "1")]
        [InlineData("unquote", "(unquote {add 1 2})", "3")]
        public void TestBuiltinFunctions(string functionName, string input, string expected)
        {
            var actual = Evaluate(input).ToString();
            Assert.Equal(expected, actual);
        }

        private INode Evaluate(string input)
        {
            var tokenizer = new Tokenizer(input);
            var parser = new Parser(tokenizer.Tokenize());
            var evaluator = new Evaluator(parser.Parse());
            return evaluator.Evaluate();
        }
    }
}
