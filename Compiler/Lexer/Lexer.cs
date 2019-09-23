using System;
using System.Collections.Generic;
using System.Linq;

namespace Lexer
{
    public class Lexer
    {
        private readonly string _code;
        private int _offset = 0;
        private string _currentValue = String.Empty;
        private char _currChar;

        private readonly List<Token> _reservedTokens = new List<Token>
        {
            new Token{Type = LexemType.KW_int, Value = "int"},
            new Token{Type = LexemType.KW_bool, Value = "bool"},
            new Token{Type = LexemType.KW_string, Value = "string"}
        };

        public Lexer(string code)
        {
            _code = code;
        }

        public Token Get()
        {
            NextChar();
            if (IsIdentChar(_currChar))
            {
                _currentValue += _currChar;
                return FinishIdent();
            }
            else
            {
                throw new ArgumentException("shiet");
            }
        }

        private Token FinishIdent()
        {
            NextChar();
            while (IsIdentChar(_currChar))
            {
                _currentValue += _currChar;
                NextChar();
            }

            return LookUp(_currentValue) ?? new Token{Type = LexemType.Ident, Value = _currentValue};
        }

        private Token LookUp(string s)
        {
            var token = _reservedTokens.SingleOrDefault(t => t.Value == s);
            if (token == null)
                return null;
            return new Token{Type = token.Type};
        }

        private bool IsIdentChar(char c) =>
            c > 'a' & c < 'z' || c > 'A' & c < 'Z' || c == '_';

        private void NextChar() => _currChar = _code[_offset++];
    }
}
