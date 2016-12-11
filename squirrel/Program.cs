using System;

namespace squirrel
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var tokenizer = new Tokenizer("(add +1 -2)");

            foreach (var token in tokenizer.GetAllTokens())
            {
                Console.WriteLine(token);
            }
        }
    }
}
