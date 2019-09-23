using System;
using System.Collections.Generic;
using System.Linq;

namespace Lexer_Implementation
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



        public IEnumerable<Token> Get()
        {
            while (true)
            {
                NextChar();
                if (IsIdentChar(_currChar))
                {
                    _currentValue += _currChar;
                    yield return FinishIdent();
                }
                else if (_currChar == 0)
                {
                    break;
                }
                else
                {
                    throw new ArgumentException("shiet");
                }
            }
            yield return new Token
            {
                Type = LexemType.EOF
            };
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

        private bool IsIdentChar(char c)
        {
            var res = c >= 'a' & c <= 'z' || c >= 'A' & c <= 'Z' || c == '_';
            return res;
        }

        private void NextChar()
        {
            if (_code.Length <= _offset)
                _currChar = (char) 0;
            else
                _currChar = _code[_offset++];
        }
    }
}
