using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lexer_Implementation.DynamicLexer.FSM
{
    public class StateMachineBuilder
    {
        public StateMachine Build(List<BNFRule> rules, List<BNFRule> helpers)
        {
            var start = new State();
            start.Transitions.Add(new Transition
            {
                From = start,
                To = start,
                Conditions = new List<char> {'\n','\r', '\t', ' ' }
            });
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
                                Transition tr;
                                if ((tr = currentState.Transitions.SingleOrDefault(t => t.Conditions.Contains(c))) != null)// jei is einamos busenos jau yra perejimas su duotu simboliu
                                {
                                    currentState = tr.To;
                                    if (i == alternative.Count - 1 && j == chars.Length - 1 && !currentState.IsFinal) // jei paskutinis einamos alternatyvos simbolis
                                    {
                                        currentState.IsFinal = true;
                                        currentState.LexemeType = lexemeType;
                                    }
                                }
                                else
                                {
                                    var newState = new State();
                                    if (i == alternative.Count - 1 && j == chars.Length - 1) // jei paskutinis einamos alternatyvos simbolis
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
            }

            return new StateMachine(start);
        }
    }
}
