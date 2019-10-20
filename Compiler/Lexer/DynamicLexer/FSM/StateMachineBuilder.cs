using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lexer_Implementation.DynamicLexer.FSM
{
    public class StateMachineBuilder
    {
        public StateMachine Build(List<BNFRule> rules)
        {
            var start = new State();
            start.Transitions['\n'] = start;
            start.Transitions['\r'] = start;
            start.Transitions['\t'] = start;
            start.Transitions[' '] = start;
            foreach (var bnfRule in rules)
            {
                AddRule(bnfRule, new List<State> { start }, true, bnfRule.Name);
            }

            return new StateMachine(start);
        }

        private List<State> AddRule(BNFRule bnfRule, List<State> startingStates, bool shouldBeFinal, string lexemeType, RecursionState tracker = null, bool isFirstStateOfRecursion = false)
        {
            var newCurrentStates = new List<State>();

            if(IsRecursion(bnfRule, out var otherRule))
            {
                List<State> unfinishedStates = startingStates;
                var finishedStates = new List<State>();
                do
                {
                    tracker = new RecursionState();
                    var someStates = AddRule(otherRule, unfinishedStates, shouldBeFinal, lexemeType, tracker, true);

                    newCurrentStates.AddRange(someStates.Where(s => s.RecursionState != null && s.RecursionState.CreatedNewStateOnFirstStep));

                    var x = someStates
                        .Where(s => !s.RecursionState.CreatedNewStateOnFirstStep)
                        .Where(s => s.RecursionState.RecursionName == bnfRule.Name).ToList();
                    finishedStates.AddRange(x);

                    unfinishedStates = someStates
                        .Where(s => !s.RecursionState.CreatedNewStateOnFirstStep)
                        .Where(s => s.RecursionState.RecursionName != bnfRule.Name)
                        .ToList();

                } while (unfinishedStates.Any());

                var firstChars = GetFirstChars(otherRule);
                foreach(var state in newCurrentStates)
                {
                    firstChars.ForEach(c => state.Transitions[c] = state.RecursionState.FirstState);
                    state.RecursionState.RecursionName = bnfRule.Name;
                }

                newCurrentStates.AddRange(finishedStates);
            }
            else
            {
                foreach (var alternative in bnfRule.Alternatives)
                {
                    var newTacker = tracker?.Clone();
                    var currentStates = startingStates;
                    for (var i = 0; i < alternative.Count; i++)
                    {
                        var rule = alternative[i];
                        if (rule.IsTerminal)
                        {
                            currentStates = AddTerminalRule(rule, currentStates, shouldBeFinal && i == alternative.Count - 1, lexemeType, newTacker, isFirstStateOfRecursion && i == 0);
                        }
                        else
                        {
                            currentStates = AddRule(rule, currentStates, shouldBeFinal && i == alternative.Count - 1, lexemeType, newTacker, isFirstStateOfRecursion && i == 0);
                        }
                    }
                    newCurrentStates.AddRange(currentStates);
                }
            }

            return newCurrentStates;
        }

        private List<char> GetFirstChars(BNFRule rule)
        {
            if (rule.IsTerminal)
                return new List<char> { rule.TerminalValue[0] };
            else
                return rule.Alternatives
                    .Select(a => a.FirstOrDefault())
                    .SelectMany(r => GetFirstChars(r))
                    .ToList();
        }

        private bool IsRecursion(BNFRule rule, out BNFRule theOtherRule)
        {
            
            if (rule.Alternatives != null && rule.Alternatives.Count == 2 ) // jei yra 2 alternatyvos ir viena ju turi dvi taisykles, is kuriu viena yra pati taisykle 'rule'
            { 
                var otherRule = rule.Alternatives.FirstOrDefault(r => r.Count == 1)?.FirstOrDefault();
                theOtherRule = otherRule;
                if (otherRule == null)
                    return false;

                if(otherRule.IsTerminal)
                    return rule.Alternatives.Any(rs => rs.Count == 2 && rs.Any(r => r.Equals(rule)) && rs.Any(r => r.TerminalValue == otherRule.TerminalValue));

                return rule.Alternatives.Any(rs => rs.Count == 2 && rs.Any(r => r.Equals(rule)) && rs.Any(r => r.Equals(otherRule)));               
            }
            theOtherRule = null;
            return false;
        }
        private static List<State> AddTerminalRule(BNFRule bnfRule, List<State> currentStates, bool shouldBeFinal, string lexemeType, RecursionState tracker = null, bool isFirstStateOfRecursion = false)
        {
            var newCurrentStates = new List<State>();

            foreach (var cs in currentStates)
            {
                var newTracker = tracker?.Clone();
                var currentState = cs;
                var chars = bnfRule.TerminalValue.ToCharArray();
                for (var j = 0; j < chars.Length; j++)
                {
                    var c = chars[j];
                    if (currentState.Transitions[c] != null)// jei is einamos busenos jau yra perejimas su duotu simboliu
                    {
                        currentState = currentState.Transitions[c];
                        if (shouldBeFinal && j == chars.Length - 1 && !currentState.IsFinal) // jei paskutinis einamos alternatyvos simbolis
                        {
                            currentState.IsFinal = true;
                            currentState.LexemeType = lexemeType;
                        }
                    }
                    else
                    {
                        var newState = new State();
                       
                        if (newTracker != null && j == 0 && isFirstStateOfRecursion)
                        {
                            newTracker.CreatedNewStateOnFirstStep = true;
                            newTracker.FirstState = newState;
                        }

                        if (shouldBeFinal && j == chars.Length - 1) // jei paskutinis einamos alternatyvos simbolis
                        {
                            newState.IsFinal = true;
                            newState.LexemeType = lexemeType;
                        }

                        currentState.Transitions[c] = newState;
                        currentState = newState;
                    }
                }
                if(newTracker != null)
                {
                    if(currentState.RecursionState == null)
                        currentState.RecursionState = newTracker;
                    else
                    {
                        currentState.RecursionState.CreatedNewStateOnFirstStep = newTracker.CreatedNewStateOnFirstStep;
                        currentState.RecursionState.FirstState = newTracker.FirstState;
                    }
                }
                newCurrentStates.Add(currentState);
            }

            return newCurrentStates;
        }
    }

    class Tracker
    {
        public bool CreatedNewStateOnFirstStep { get; set; }

        public Tracker Clone()
        {
            return new Tracker { CreatedNewStateOnFirstStep = this.CreatedNewStateOnFirstStep };
        }
    }
}
