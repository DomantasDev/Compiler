﻿using System.Collections.Generic;
using Lexer_Contracts;
using Lexer_Implementation.DynamicLexer;

namespace Parser_Implementation.Lexemes
{
    public class LexemeSource
    {
        private readonly List<Token> _lexemes;
        private int _offset = 0;

        public LexemeSource(List<Token> lexemes)
        {
            _lexemes = lexemes;
        }

        public ExpectResult Expect(string lexemeName)
        {
            if (_lexemes.Count == _offset)
                return new ExpectResult();

            if (_lexemes[_offset].Type == lexemeName)
            {
                var res = new ExpectResult(true ,_lexemes[_offset].ToNode());
                _offset++;
                return res;
            }
            
            return new ExpectResult();
        }

        public int SetCheckpoint()
        {
            return _offset;
        }

        public void RevertCheckPoint(int checkpoint)
        {
            _offset = checkpoint;
        }
    }
}
