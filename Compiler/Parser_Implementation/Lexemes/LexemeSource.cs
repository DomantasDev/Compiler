using System.Collections.Generic;
using Lexer_Implementation.DynamicLexer;

namespace Parser_Implementation.Lexemes
{
    public class LexemeSource
    {
        private readonly List<Lexeme> _lexemes;
        private int _offset = 0;
        private readonly Stack<int> _checkpoints;

        public LexemeSource(List<Lexeme> lexemes)
        {
            _lexemes = lexemes;
            _checkpoints = new Stack<int>();
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

        public void SetCheckpoint()
        {
            _checkpoints.Push(_offset);
        }

        public void RevertCheckPoint()
        {
            _offset = _checkpoints.Pop();
        }
    }
}
