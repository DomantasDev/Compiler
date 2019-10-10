using System;
using System.Collections.Generic;
using System.Text;

namespace Lexer_Implementation.DynamicLexer
{
    public class Lexeme
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public int Line { get; set; }
    }
}
