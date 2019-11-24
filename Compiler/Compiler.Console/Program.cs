using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AbstractSyntaxTree_Implementation;
using Lexer_Implementation;
using Lexer_Implementation.DynamicLexer;
using Lexer_Implementation.StaticLexer;
using Parser_Implementation;
using Parser_Implementation.BnfReader;
using Parser_Implementation.Lexemes;
using Token = Lexer_Implementation.DynamicLexer.Token;

namespace ConsoleApp
{
    // sutvarkyt member access expr, if, ctor, (null meta true
    class Program
    {
        static void Main(string[] args)
        {
            //var lexemes = new DynamicLexer("../../../DynamicLexer/lexemes.bnf")
            //    .GetLexemes(File.ReadAllText("../../../DynamicLexer/code.txt"));
            //Print(lexemes);

            var parser = new Parser();
            var result = parser.Parse(out var node);

            Console.WriteLine(result);

            node.Print(new NodePrinter());
            Console.ReadLine();
        }

        private static void Print(IEnumerable<Token> lexemes)
        {
            Console.WriteLine($"{"Line".PadLeft(5)}|{"Type".PadRight(20)}| Value");
            Console.WriteLine($"{new string('-', 5)}+{new string('-', 20)}+{new string('-', 20)}");
            try
            {
                foreach (var lexeme in lexemes)
                {
                    Print(lexeme);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void Print(Token lexeme)
        {
            Console.Write($"{lexeme.Line.ToString().PadRight(5)}|{lexeme.Type.ToString().PadRight(20)}|");
            Console.WriteLine($"{lexeme.Value}");
        }
    }
}
