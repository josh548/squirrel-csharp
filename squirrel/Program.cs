using System;
using System.IO;
using Squirrel.Exceptions;
using Squirrel.Nodes;

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
            var text = File.ReadAllText(path);
            var env = new Environment();
            INode result;
            try
            {
                result = Interpret(text, ref env);
            }
            catch (TokenizerException e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            catch (ParserException e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            Console.WriteLine(result);
        }

        private static void RunInteractiveConsole()
        {
            var env = new Environment();

            const string prompt = ">>> ";
            var lastOutput = "";

            Console.WriteLine("Press ^D to quit. Use an underscore ('_') to refer to the last output.");
            while (true)
            {
                Console.Write(prompt);

                var line = Console.ReadLine();
                if (line == null)
                {
                    break;
                }

                line = line.Replace("_", lastOutput);
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                INode result;
                try
                {
                    result = Interpret(line, ref env);
                }
                catch (TokenizerException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (ParserException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                Console.WriteLine(result);
                lastOutput = result.ToString();
            }
        }

        private static INode Interpret(string text, ref Environment env)
        {
            var tokenizer = new Tokenizer(text);
            var parser = new Parser(tokenizer.Tokenize());
            var evaluator = new Evaluator(parser.Parse());
            return evaluator.Evaluate(ref env);
        }
    }
}
