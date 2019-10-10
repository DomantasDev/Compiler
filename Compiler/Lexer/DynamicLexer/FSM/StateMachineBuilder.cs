using System;
using System.Collections.Generic;
using System.Text;

namespace Lexer_Implementation.DynamicLexer.FSM
{
    public class StateMachineBuilder
    {
        public StateMachine Build(List<BNFRule> rules, List<BNFRule> helpers)
        {
            var start = new State();
            State currentState;
            foreach (var bnfRule in rules)
            {
                var lexemeType = bnfRule.Name;
                foreach (var alternative in bnfRule.Alternatives)
                {
                    currentState = start;
                    for (var i = 0; i < alternative.Count; i++)
                    {
                        var rule = alternative[i];
                        if (rule.IsTerminal)
                        {
                            var chars = rule.TerminalValue.ToCharArray();
                            for (var j = 0; j < chars.Length; j++)
                            {
                                var c = chars[j];
                                var newState = new State();
                                if (i == alternative.Count - 1 && j == chars.Length - 1)
                                {
                                    newState.IsFinal = true;
                                    newState.LexemeType = lexemeType;
                                }
                                var newTransition = new Transition
                                {
                                    To = newState,
                                    From = currentState,
                                    Conditions = new List<char> {c}
                                };
                                currentState.Transitions.Add(newTransition);
                                currentState = newState;
                            }
                        }
                    }
                }
            }

            return new StateMachine(start);
        }
    }
}
