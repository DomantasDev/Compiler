using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AbstractSyntaxTree_Implementation;
using AbstractSyntaxTree_Implementation.ResolveNames;
using Common;
using Lexer_Contracts;
using Lexer_Implementation;
using Lexer_Implementation.DynamicLexer;
using Parser_Implementation;
using Parser_Implementation.BnfReader;
using Parser_Implementation.BnfRules.Alternatives;
using Parser_Implementation.Lexemes;

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

            var code = "../../../DynamicLexer/code.txt";

            ErrorWriter.File = code;

            var parser = new Parser(code);
            var result = parser.Parse(out var root);

            Console.WriteLine(result);

            //root.Print(new NodePrinter());
            //Console.WriteLine("\n" + new string('-', 20) + "\n");

            var scope = new Scope(null);
            root.ResolveNames(scope);

            Console.WriteLine();

            root.CheckTypes();

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
