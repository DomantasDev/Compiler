using System;

namespace Lexer_Contracts
{
    public class Token
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public int Line { get; set; }
    }
}
