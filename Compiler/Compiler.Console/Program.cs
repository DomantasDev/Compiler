using System;
using System.Collections.Generic;
using AbstractSyntaxTree_Implementation.ResolveNames;
using CodeExecution;
using CodeGeneration.CodeGeneration;
using Common;
using Lexer_Contracts;
using Parser_Implementation;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //var lexemes = new DynamicLexer("../../../DynamicLexer/lexemes.bnf")
            //    .GetLexemes(File.ReadAllText("../../../DynamicLexer/code.txt"));
            //Print(lexemes);

            var code = "../../../DynamicLexer/asd.txt";

            ErrorWriter.File = code;

            var parser = new Parser(code);
            var success = parser.Parse(out var root);

            if(!success)
                Console.WriteLine("Parsing failed");

            //root.Print(new NodePrinter());
            //Console.WriteLine("\n" + new string('-', 20) + "\n");

            if (success)
            {
                var scope = new Scope(null);
                root.ResolveNames(scope);

                //if (ErrorWriter.ErrorCount == 0)
                    root.CheckTypes();

                if (ErrorWriter.ErrorCount == 0)
                {
                    var codeWriter = new CodeWriter();
                    root.GenerateCode(codeWriter);
                    codeWriter.Disassemble();

                    //Console.WriteLine($"code length: {codeWriter.Code.Count}");

                    var vm = new VirtualMachine(codeWriter.Code.ToArray());
                    try
                    {
                        vm.Execute();
                    }
                    catch (Exception e)
                    {
                        if(e.Message != "")
                            e.Message.RaiseError();
                    }
                }
            }
            //Console.ReadLine();
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
