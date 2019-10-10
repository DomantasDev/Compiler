using System;
using System.Collections.Generic;
using System.Text;

namespace Lexer_Implementation.DynamicLexer.FSM
{
    public class State
    {
        public bool IsFinal { get; set; }
        public string LexemeType { get; set; }
        public List<Transition> Transitions { get; set; } = new List<Transition>();
    }
}
