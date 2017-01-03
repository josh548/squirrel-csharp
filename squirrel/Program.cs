using System;
using System.Collections.Generic;
using System.IO;
using Squirrel.Exceptions;
using Squirrel.Nodes;
using Squirrel.Tokens;

namespace Squirrel
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                RunInteractiveConsole();
            }
            else
            {
                RunFile(args[0]);
            }
        }

        private static void RunFile(string path)
        {
            var environment = new Environment();
            Interpret(File.ReadAllText(path), ref environment);
        }

        private static void RunInteractiveConsole()
        {
            var env = new Environment();

            const string prompt = ">>> ";

            Console.WriteLine("Press ^D to quit");
            while (true)
            {
                Console.Write(prompt);

                var line = Console.ReadLine();
                if (line == null)
                {
                    break;
                }
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                Interpret(line, ref env);
            }
        }

        private static void Interpret(string text, ref Environment env)
        {
            var tokenizer = new Tokenizer(text);

            List<Token> tokens;
            try
            {
                tokens = tokenizer.Tokenize();
            }
            catch (TokenizerException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            var parser = new Parser(tokens);

            INode root;
            try
            {
                root = parser.Parse();
            }
            catch (ParserException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            var evaluator = new Evaluator(root);

            Console.WriteLine(evaluator.Evaluate(ref env));
        }
    }
}
