using System;
using System.Collections.Generic;
using System.Text;

namespace Lexer_Implementation.DynamicLexer.FSM
{
    public class Transition
    {
        public State From { get; set; }
        public State To { get; set; }
        public List<char> Conditions { get; set; }
    }
}
