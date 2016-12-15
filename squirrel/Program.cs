using System;

namespace squirrel
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var tokenizer = new Tokenizer("(add 1 2 3)");
            var parser = new Parser(tokenizer);
            var root = parser.Parse();
            var evaluator = new Evaluator(root);
            var result = evaluator.Evaluate();
            Console.WriteLine(result);
        }
    }
}
