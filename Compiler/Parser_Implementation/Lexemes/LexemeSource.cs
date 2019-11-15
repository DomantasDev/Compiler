using System.Collections.Generic;
using Lexer_Implementation.DynamicLexer;

namespace Parser_Implementation.Lexemes
{
    public class LexemeSource
    {
        private readonly List<Lexeme> _lexemes;
        private int _offset = 0;

        public LexemeSource(List<Lexeme> lexemes)
        {
            _lexemes = lexemes;
        }

        public bool Expect(string lexemeName)
        {
            if (_lexemes.Count == _offset)
                return false;

            if (_lexemes[_offset++].Type == lexemeName)
                return true;

            _offset--;
            return false;
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
