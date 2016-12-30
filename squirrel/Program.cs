using System;

namespace Squirrel
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("no file specified");
                return;
            }

            var input = System.IO.File.ReadAllText(args[0]);
            var tokenizer = new Tokenizer(input);
            var tokens = tokenizer.Tokenize();
            var parser = new Parser(tokens);
            var evaluator = new Evaluator(parser.Parse());
            Console.WriteLine(evaluator.Evaluate());
        }
    }
}
