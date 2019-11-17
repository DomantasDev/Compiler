using System;
using System.Collections.Generic;
using System.Text;
using Lexer_Implementation.DynamicLexer;

namespace Parser_Implementation.Lexemes
{
    public class LexemeExpectResult
    {
        public LexemeExpectResult(Lexeme lexeme)
        {
            Lexeme = lexeme;
            Success = true;
        }

        public LexemeExpectResult()
        {
            
        }
        public bool Success { get; set; }
        public Lexeme Lexeme { get; set; }
    }
}
