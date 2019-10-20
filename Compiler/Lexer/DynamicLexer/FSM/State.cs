using System;
using System.Collections.Generic;
using System.Text;

namespace Lexer_Implementation.DynamicLexer.FSM
{
    public class State
    {
        public bool IsFinal { get; set; }
        public string LexemeType { get; set; }
        public State[] Transitions { get; set; } = new State[256];

        internal RecursionState RecursionState{ get; set; }
    }
}
