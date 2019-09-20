using System;

namespace Lexer
{
    public class Lexer
    {
        private readonly string _code;
        private int _offset = 0;

        public Lexer(string code)
        {
            _code = code;
        }

        public Token Get()
        {
            char x = _code[_offset++];
            if (x > 'a' & x < 'z' || x > 'A' & x < 'Z' || x == '_')
            {

            }
        }

    }
}
