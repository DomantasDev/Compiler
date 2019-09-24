using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lexer_Implementation
{
    public class Lexer
    {
        private readonly string _code;
        private int _offset = 0;
        private int _line = 1;
        private string _currentValue = String.Empty;
        private char _currChar;

        private readonly List<Token> _reservedTokens = new List<Token>
        {
            new Token{Type = LexemType.KW_int, Value = "int"},  
            new Token{Type = LexemType.KW_bool, Value = "bool"},
            new Token{Type = LexemType.KW_string, Value = "string"}
        };

        private readonly List<char> _escapableChars = new List<char>{'n', '\\', 't'};

        public Lexer(string code)
        {
            _code = code;
        }



        public IEnumerable<Token> Get()
        {
            while (true)
            {   
                NextChar();
                if (IsWhiteSpace(_currChar))
                {
                    continue;
                }
                if (IsIdentChar(_currChar))
                {
                    yield return FinishIdent();
                }
                else if (_currChar == '\"')
                {
                    yield return FinishString();
                }
                else if (IsDigit(_currChar))
                {
                    // 123 1.2 1. .1
                    yield return FinishNumber();
                }
                else if (_currChar == '.')
                {
                    yield return FinishDot();
                }
                else if (_currChar == 0)
                {
                    break;
                }
                else
                {
                    throw new ArgumentException("shiet");
                }
                _currentValue = String.Empty;
            }
            yield return new Token
            {
                Type = LexemType.EOF
            };
        }

        private Token FinishDot()
        {
            _currentValue += _currChar;
            NextChar();
            if (IsDigit(_currChar))
                return FinishNumber(true);

            _offset--;
            return new Token
            {
                Type = LexemType.Op_dot
            };
        }

        private Token FinishNumber(bool isFloat = false)
        {
            while (true)
            {
                if (IsDigit(_currChar))
                {
                    _currentValue += _currChar;
                }
                else if (_currChar == '.')
                {
                    if(isFloat)
                        throw new ArgumentException("Unexpected \".\" encountered");
                    _currentValue += _currChar;
                    isFloat = true;
                } 
                else 
                    break;
                NextChar();
            }
            return new Token
            {
                Type = isFloat ? LexemType.Lit_float : LexemType.Lit_int,
                Value = _currentValue
            };
        }

        private Token FinishString()
        {
            while (true)
            {
                NextChar();
                if (_currChar == 0 || _currChar == '\n')
                    throw new ArgumentException("\" expected");
                if(_currChar == '\"')
                    break;
                if (_currChar == '\\')
                {
                    NextChar();
                    if(!IsEscapedChar(_currChar))
                        throw new ArgumentException($"unrecognized escape sequence: \\{_currChar}");
                    _currentValue += @"\";
                }

                _currentValue += _currChar;
            }
            return new Token
            {
                Type = LexemType.Lit_string,
                Value = _currentValue

        };
        }

        private Token FinishIdent()
        {
            _currentValue += _currChar;
            NextChar();
            while (IsIdentChar(_currChar) || IsDigit(_currChar)) 
            {
                _currentValue += _currChar;
                NextChar();
            }

            _offset--;
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
            return c >= 'a' & c <= 'z' || c >= 'A' & c <= 'Z' || c == '_';
        }

        private bool IsEscapedChar(char c)
        {
            return _escapableChars.Any(x => x == c);
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool IsWhiteSpace(char c)
        {
            return string.IsNullOrWhiteSpace(c.ToString());
        }
        
        private void NextChar()
        {

            if (_code.Length <= _offset)
            {
                _currChar = (char) 0;
                return;
            }
            
            _currChar = _code[_offset++];

            if (_currChar == '\n')
                _line++;
        }
    }
}
