using System;
using System.Collections.Generic;
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

            var include = app.Option(
                template: "-i | --include <directory>",
                description: "add the directory to the list of directories to be searched for " +
                             "modules, which includes by default the current directory as well " +
                             "as the directory of the file argument, if provided",
                optionType: CommandOptionType.MultipleValue
            );

            var file = app.Argument(
                name: "file",
                description: "source file to run",
                multipleValues: false
            );

            app.HelpOption("-h | --help");

            app.OnExecute(() =>
            {
                var includeDirs = include.HasValue() ? include.Values : new List<string>();
                includeDirs.Add(Directory.GetCurrentDirectory());

                if (string.IsNullOrEmpty(file.Value))
                {
                    RunInteractiveConsole(includeDirs);
                }
                else
                {
                    includeDirs.Add(Path.GetDirectoryName(Path.GetFullPath(file.Value)));
                    RunFile(file.Value, includeDirs);
                }

                return 0;
            });

            app.Execute(args);
        }

        private static void RunFile(string path, List<string> includeDirs)
        {
            var text = File.ReadAllText(path);
            var env = new Squirrel.Environment();
            INode result;
            try
            {
                result = Interpret(text, ref env, includeDirs);
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

            if (!result.Equals(Evaluator.Null))
            {
                Console.WriteLine(result);
            }
        }

        private static void RunInteractiveConsole(List<string> includeDirs)
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
                    result = Interpret(line, ref env, includeDirs);
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

                if (!result.Equals(Evaluator.Null))
                {
                    Console.WriteLine(result);
                }

                if (result.GetType() != typeof(ErrorNode))
                {
                    env.Put("_", result);
                }
            }
        }

        private static INode Interpret(string text, ref Squirrel.Environment env, List<string> includeDirs)
        {
            var tokenizer = new Tokenizer(text);
            var parser = new Parser(tokenizer.Tokenize());
            var evaluator = new Evaluator(parser.Parse(), includeDirs);
            return evaluator.Evaluate(ref env);
        }
    }
}
