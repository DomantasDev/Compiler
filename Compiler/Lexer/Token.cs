using System;
using System.Collections.Generic;
using System.Text;

namespace Lexer
{
    public class Token
    {
        public string Value { get; set; }
        public int Line { get; set; }
        public LexemType Type { get; set; }
    }
}
