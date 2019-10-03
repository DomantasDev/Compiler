﻿using System;
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

            Console.WriteLine($"{"Line".PadLeft(5)}|{"Type".PadRight(15)}| Value");
            Console.WriteLine($"{new string('-', 5)}+{new string('-', 15)}+{new string('-', 20)}");

            try
            {
                foreach (var token in lexer.Get())
                {
                    Print(token);
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }

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
    }
}
