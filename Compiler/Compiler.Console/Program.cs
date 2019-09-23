using System;
using Lexer_Implementation;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var code = "asd1";

            var lexer = new Lexer(code);
            var res = lexer.Get();

            Print(res);
            Console.ReadLine();
        }

        static void Print(Token token)
        {
            Console.WriteLine(token.Value);
            Console.WriteLine(token.Type);
        }
    }
}
