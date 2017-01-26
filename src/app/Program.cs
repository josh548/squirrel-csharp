using System;
using System.IO;
using Microsoft.Extensions.CommandLineUtils;
using Squirrel;
using Squirrel.Exceptions;
using Squirrel.Nodes;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = new CommandLineApplication();

            var file = app.Argument(
                name: "file",
                description: "source file to run",
                multipleValues: false
            );

            app.HelpOption("-h | --help");

            app.OnExecute(() =>
            {
                if (string.IsNullOrEmpty(file.Value))
                {
                    RunInteractiveConsole();
                }
                else
                {
                    RunFile(file.Value);
                }

                return 0;
            });

            app.Execute(args);
        }

        private static void RunFile(string path)
        {
            var text = File.ReadAllText(path);
            var env = new Squirrel.Environment();
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

            if (!result.Equals(Evaluator.Null)) {
                Console.WriteLine(result);
            }
        }

        private static void RunInteractiveConsole()
        {
            var env = new Squirrel.Environment();

            const string prompt = ">>> ";

            Console.WriteLine("Press ^D to quit. Use an underscore ('_') to refer to the last output.");
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

                if (!result.Equals(Evaluator.Null)) {
                    Console.WriteLine(result);
                }

                if (result.GetType() != typeof(ErrorNode))
                {
                    env.Put("_", result);
                }
            }
        }

        private static INode Interpret(string text, ref Squirrel.Environment env)
        {
            var tokenizer = new Tokenizer(text);
            var parser = new Parser(tokenizer.Tokenize());
            var evaluator = new Evaluator(parser.Parse());
            return evaluator.Evaluate(ref env);
        }
    }
}
