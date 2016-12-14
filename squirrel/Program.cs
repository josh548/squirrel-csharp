using System;

namespace squirrel
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var tokenizer = new Tokenizer("(eval {add 1 2})");
            var parser = new Parser(tokenizer);
            var root = parser.Parse();
            var grapher = new DotGrapher(root);
            var graph = grapher.GenerateGraph();
            Console.WriteLine(graph);
        }
    }
}
