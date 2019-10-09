using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Lexer_Implementation.DynamicLexer.FSM
{
    public class StateMachine
    {
        private readonly State _startState;
        private State _currentState;
        public StateMachine(State startState)
        {
            _startState = _currentState = startState;
        }
        public void Reset()
        {
            _currentState = _startState;
        }
    }
}
