using System;
using System.IO;
using System.Text.RegularExpressions;
using Lexer_Implementation;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var code = File.ReadAllText("../../../code.txt");

            var lexer = new Lexer(code);
            foreach (var token in lexer.Get())
            {
                Print(token);
            }

            Console.ReadLine();
        }

        static void Print(Token token)
        {
            Console.Write($"Type:{token.Type.ToString().PadLeft(15)}");
            if (token.Value != null)
            {
                if(token.Type == LexemType.Lit_string)
                    Console.Write($"\tvalue:\t{(token.Value)}");
                else
                    Console.Write($"\tvalue:\t{token.Value}");
            }
                
            Console.WriteLine();
        }
    }
}
