using System;
using System.IO;
using System.Text.RegularExpressions;
using Lexer_Implementation;
using Lexer_Implementation.DynamicLexer;
using Lexer_Implementation.StaticLexer;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var dynamicLexer = new DynamicLexer("../../../DynamicLexer/lexemes.bnf");

            Console.WriteLine($"{"Line".PadLeft(5)}|{"Type".PadRight(15)}| Value");
            Console.WriteLine($"{new string('-', 5)}+{new string('-', 15)}+{new string('-', 20)}");
            try
            {
                foreach (var lexeme in dynamicLexer.GetLexemes(File.ReadAllText("../../../DynamicLexer/code.txt")))
                {
                    Print(lexeme);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //var code = File.ReadAllText("../../../code.txt");

            //var lexer = new Lexer(code);

            //Console.WriteLine($"{"Line".PadLeft(5)}|{"Type".PadRight(15)}| Value");
            //Console.WriteLine($"{new string('-', 5)}+{new string('-', 15)}+{new string('-', 20)}");

            //try
            //{
            //    foreach (var token in lexer.Get())
            //    {
            //        Print(token);
            //    }
            //}
            //catch (ArgumentException ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

            Console.ReadLine();
        }

        static void Print(Token token)
        {
            Console.Write($"{token.Line.ToString().PadRight(5)}|{token.Type.ToString().PadRight(15)}|");
            if (token.Value != null)
            {
                if(token.Type == LexemType.Lit_string)
                    Console.Write($"{(token.Value)}");
                else
                    Console.Write($"{token.Value}");
            }
                
            Console.WriteLine();
        }

        static void Print(Lexeme lexeme)
        {
            Console.Write($"{lexeme.Line.ToString().PadRight(5)}|{lexeme.Type.ToString().PadRight(15)}|");
            Console.WriteLine($"{lexeme.Value}");
        }
    }
}
