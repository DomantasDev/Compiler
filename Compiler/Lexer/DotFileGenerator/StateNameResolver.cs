using Lexer_Implementation.DynamicLexer.FSM;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lexer_Implementation.DotFileGenerator
{
    public class StateNameResolver
    {
        private readonly StateCounter _counter;
        private readonly Dictionary<State, string> _stateNameMap;

        public StateNameResolver(StateCounter counter)
        {
            _counter = counter;
            _stateNameMap = new Dictionary<State, string>();
        }

        public string ResolveName(State state)
        {
            if (_stateNameMap.TryGetValue(state, out var value))
                return value;
            
            var name = state.LexemeType == null ? string.Empty : state.LexemeType + "_";

            var newName = name + _counter.GetCount(name);
            _stateNameMap.Add(state, newName);
            return newName;
        }
    }
}
