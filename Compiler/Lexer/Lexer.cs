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

        private readonly List<Token> _reservedKeyWords = new List<Token>
        {
            new Token{Type = LexemType.KW_void, Value = "void"},
            new Token{Type = LexemType.KW_int, Value = "int"},  
            new Token{Type = LexemType.KW_float, Value = "float"},
            new Token{Type = LexemType.KW_bool, Value = "bool"},
            new Token{Type = LexemType.KW_string, Value = "string"},
            new Token{Type = LexemType.KW_break, Value = "break"},
            new Token{Type = LexemType.KW_continue, Value = "continue"},
            new Token{Type = LexemType.KW_class, Value = "class"},
            new Token{Type = LexemType.KW_if, Value = "if"},
            new Token{Type = LexemType.KW_else, Value = "else"},
            new Token{Type = LexemType.KW_while, Value = "while"},
            new Token{Type = LexemType.KW_return, Value = "return"},
            new Token{Type = LexemType.KW_virtual, Value = "virtual"},
            new Token{Type = LexemType.KW_override, Value = "override"},
            new Token{Type = LexemType.KW_private, Value = "private"},
            new Token{Type = LexemType.KW_protected, Value = "protected"},
            new Token{Type = LexemType.KW_public, Value = "public"},
            new Token{Type = LexemType.KW_this, Value = "this"},
            new Token{Type = LexemType.KW_base, Value = "base"},
            new Token{Type = LexemType.Lit_bool, Value = "true"},
            new Token{Type = LexemType.Lit_bool, Value = "false"},
            new Token{Type = LexemType.KW_constructor, Value = "constructor"},
            new Token{Type = LexemType.KW_write, Value = "write"},
            new Token{Type = LexemType.KW_read, Value = "read"},
            new Token{Type = LexemType.KW_extends, Value = "extends"},
            new Token{Type = LexemType.KW_null, Value = "null"},
            //new Token{Type = LexemType., Value = ""},
        };

        private readonly List<Token> _reservedTokens = new List<Token>
        {
            new Token{Type = LexemType.Op_mult, Value = "*"},
            new Token{Type = LexemType.Op_div, Value = "/"},
            new Token{Type = LexemType.Op_plus, Value = "+"},
            new Token{Type = LexemType.Op_minus, Value = "-"},
            new Token{Type = LexemType.Op_or, Value = "|"},
            new Token{Type = LexemType.Op_and, Value = "&"},
            new Token{Type = LexemType.Op_not, Value = "!"},
            new Token{Type = LexemType.Op_brace_o, Value = "{"},
            new Token{Type = LexemType.Op_brace_c, Value = "}"},
            new Token{Type = LexemType.Op_paren_o, Value = "("},
            new Token{Type = LexemType.Op_paren_c, Value = ")"},
            new Token{Type = LexemType.Op_equal, Value = "="},
            new Token{Type = LexemType.Op_comma, Value = ","},
            new Token{Type = LexemType.OP_semicol, Value = ";"},
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
                else if (_currChar == '>')
                {
                    _currentValue += _currChar;
                    NextChar();
                    if (_currChar == '=')
                    {
                        _currentValue += _currChar;
                        yield return new Token
                        {
                            Line = _line,
                            Type = LexemType.Op_greater_e
                        };
                    }
                    else
                    {
                        _offset--;
                        yield return new Token
                        {
                            Line = _line,
                            Type = LexemType.Op_greater
                        };
                    }
                }
                else if (_currChar == '<')
                {
                    _currentValue += _currChar;
                    NextChar();
                    if (_currChar == '=')
                    {
                        _currentValue += _currChar;
                        yield return new Token
                        {
                            Line = _line,
                            Type = LexemType.Op_less_e
                        };
                    }
                    else if (_currChar == '>')
                    {
                        _currentValue += _currChar;
                        yield return new Token
                        {
                            Line = _line,
                            Type = LexemType.Op_not_equal
                        };
                    }
                    else
                    {
                        _offset--;
                        yield return new Token
                        {
                            Line = _line,
                            Type = LexemType.Op_less
                        };
                    }
                }
                else if (_currChar == ':')
                {
                    _currentValue += _currChar;
                    NextChar();
                    if (_currChar == '=')
                    {
                        _currentValue += _currChar;
                        yield return new Token
                        {
                            Line = _line,
                            Type = LexemType.Op_assign
                        };
                    }
                    else
                    {
                        _offset--;
                        yield return new Token
                        {
                            Line = _line,
                            Type = LexemType.Op_col
                        };
                    }
                }
                else if (MaybeReserved(out var token))
                {
                    yield return token;
                }
                else if (_currChar == '#')
                {
                    RemoveComment();
                    continue;
                }
                else if (_currChar == 0)
                {
                    break;
                }
                else
                {
                    throw new ArgumentException("Ooops");
                }
                _currentValue = String.Empty;
            }
            yield return new Token
            {
                Type = LexemType.EOF
            };
        }

        private void RemoveComment()
        {
            NextChar();
            if (_currChar == '#')
            {
                do
                    NextChar();
                while (_currChar != '\n');
            }
            else if (_currChar == '*')
            {
                do
                {
                    NextChar();
                    if (_currChar == '*')
                    {
                        NextChar();
                        if(_currChar == '#')
                            break;
                    }
                } while (true);
            }
            else 
                throw new ArgumentException("expected '#' or '*'");
        }

        private bool MaybeReserved(out Token token)
        {
            token = _reservedTokens.SingleOrDefault(t => t.Value == _currChar.ToString());
            if (token == null)
                return false;

            token = new Token
            {
                Line = _line,
                Type = token.Type
            };

            return true;

        }

        private Token FinishDot()
        {
            _currentValue += _currChar;
            NextChar();
            if (IsDigit(_currChar))
            {
                return FinishFloat();
            }

            _offset--;
            return new Token
            {
                Type = LexemType.Op_dot
            };
        }

        private Token FinishNumber()
        {
            while (true)
            {
                if (IsDigit(_currChar))
                {
                    _currentValue += _currChar;
                }
                else if (_currChar == '.')
                {
                    return FinishFloat();
                }
                else
                {
                    _offset--;
                    break;
                }
                NextChar();
            }
            return new Token
            {
                Line = _line,
                Type = LexemType.Lit_int,
                Value = _currentValue
            };
        }

        private Token FinishFloat()
        {
            _currentValue += _currChar;
            NextChar();
            while (true)
            {
                if (IsDigit())
                {
                    _currentValue += _currChar;
                    NextChar();
                }
                else if (_currChar == 'e')
                {
                    return FinishExponent();
                }
                else
                {
                    _offset--;
                    break;
                }
            }

            return new Token
            {
                Line = _line,
                Type = LexemType.Lit_float,
                Value = _currentValue
            };
        }

        private Token FinishExponent()
        {
            _currentValue += _currChar;
            NextChar();
            if (_currChar == '-')
            {
                _currentValue += _currChar;
                NextChar();
            }

            if (!IsDigit())
            {
                throw new ArgumentException("expected a digit");
            }

            do
            {
                _currentValue += _currChar;
                NextChar();
            }
            while (IsDigit());

            _offset--;

            return new Token
            {
                Line = _line,
                Type = LexemType.Lit_float,
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
                    if(!IsEscapedChar())
                        throw new ArgumentException($"unrecognized escape sequence: \\{_currChar}");
                    _currentValue += @"\";
                }

                _currentValue += _currChar;
            }

            return new Token
            {
                Line = _line,
                Type = LexemType.Lit_string,
                Value = _currentValue
            };
        }

        private Token FinishIdent()
        {
            _currentValue += _currChar;
            NextChar();
            while (IsIdentChar() || IsDigit()) 
            {
                _currentValue += _currChar;
                NextChar();
            }

            _offset--;
            return LookUp(_currentValue) ?? new Token
            {
                Line = _line,
                Type = LexemType.Ident,
                Value = _currentValue
            };
        }

        private Token LookUp(string s)
        {
            var token = _reservedKeyWords.SingleOrDefault(t => t.Value == s);
            if (token == null)
                return null;

            var newToken = new Token { Type = token.Type };

            if (newToken.Type == LexemType.Lit_bool)
                newToken.Value = token.Value;

            return newToken;
        }

        private bool IsIdentChar(char c)
        {
            return c >= 'a' & c <= 'z' || c >= 'A' & c <= 'Z' || c == '_';
        }

        private bool IsIdentChar()
        {
            return IsIdentChar(_currChar);
        }

        private bool IsEscapedChar(char c)
        {
            return _escapableChars.Any(x => x == c);
        }

        private bool IsEscapedChar()
        {
            return IsEscapedChar(_currChar);
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool IsDigit()
        {
            return IsDigit(_currChar);
        }

        private bool IsWhiteSpace(char c)
        {
            return string.IsNullOrWhiteSpace(c.ToString());
        }

        private bool IsWhiteSpace()
        {
            return IsWhiteSpace(_currChar);
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
