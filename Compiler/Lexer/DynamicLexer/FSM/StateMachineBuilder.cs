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
            foreach (var bnfRule in rules)
            {
                AddRule(bnfRule, new List<State> { start }, true, bnfRule.Name);
            }

            return new StateMachine(start);
        }

        private List<State> AddRule(BNFRule bnfRule, List<State> startingStates, bool shouldBeFinal, string lexemeType, Tracker tracker = null, bool isFirstStateOfRecursion = false)
        {
            var newCurrentStates = new List<State>();

            if(IsRecursion(bnfRule, out var otherRule))
            {
                
                //List<State> someStates;
                List<State> unfinishedStates = startingStates;
                var finishedStates = new List<State>();
                do
                {
                    tracker = new Tracker();
                    var someStates = AddRule(otherRule, unfinishedStates, shouldBeFinal, lexemeType, tracker, true);

                    newCurrentStates.AddRange(someStates.Where(s => s.RecursionFinished));

                    finishedStates.AddRange(someStates
                        .Where(s => !s.RecursionFinished)
                        .Where(s => s.RecursionName == bnfRule.Name));

                    unfinishedStates = someStates
                        .Where(s => !s.RecursionFinished)
                        .Where(s => s.RecursionName != bnfRule.Name)
                        .ToList();

                } while (unfinishedStates.Any());

                var firstChars = GetFirstChars(otherRule);
                foreach(var state in newCurrentStates)
                {
                    state.Transitions.Add(new Transition
                    {
                        From = state,
                        To = state,
                        Conditions = firstChars
                    });
                    state.RecursionName = bnfRule.Name;
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

        //private List<State> AddRecursion(BNFRule rule, List<State> currentStates, bool shouldBeFinal, string lexemeType)
        //{
        //    if (rule.IsTerminal)
        //    {
        //        return AddTerminalRuleForRecursion(rule, currentStates, shouldBeFinal, lexemeType, true);
        //    }
        //    else
        //    {

        //    }
        //}

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
            
            if ( rule.Alternatives.Count == 2 ) // jei yra 2 alternatyvos ir viena ju turi dvi taisykles, is kuriu viena yra pati taisykle 'rule'
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
        private static List<State> AddTerminalRule(BNFRule bnfRule, List<State> currentStates, bool shouldBeFinal, string lexemeType, Tracker tracker = null, bool isFirstStateOfRecursion = false)
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
                    Transition tr;
                    if ((tr = currentState.Transitions.SingleOrDefault(t => t.Conditions.Contains(c))) != null)// jei is einamos busenos jau yra perejimas su duotu simboliu
                    {
                        currentState = tr.To;
                        if (shouldBeFinal && j == chars.Length - 1 && !currentState.IsFinal) // jei paskutinis einamos alternatyvos simbolis
                        {
                            currentState.IsFinal = true;
                            currentState.LexemeType = lexemeType;
                        }
                    }
                    else
                    {
                        if (newTracker != null && j == 0 && isFirstStateOfRecursion)
                        {
                            newTracker.CreatedNewStateOnFirstStep = true;
                        }

                        var newState = new State();
                        if (shouldBeFinal && j == chars.Length - 1) // jei paskutinis einamos alternatyvos simbolis
                        {
                            newState.IsFinal = true;
                            newState.LexemeType = lexemeType;
                        }
                        var newTransition = new Transition
                        {
                            To = newState,
                            From = currentState,
                            Conditions = new List<char> { c }
                        };
                        currentState.Transitions.Add(newTransition);
                        currentState = newState;
                    }
                }
                if(newTracker != null)
                {
                    currentState.RecursionFinished = newTracker.CreatedNewStateOnFirstStep;
                }
                newCurrentStates.Add(currentState);
            }

            return newCurrentStates;
        }

        //private static List<State> AddTerminalRuleForRecursion(BNFRule bnfRule, List<State> currentStates, bool shouldBeFinal, string lexemeType, bool hasFirstChar)
        //{
        //    var newCurrentStates = new List<State>();

        //    foreach (var cs in currentStates)
        //    {
        //        var currentState = cs;
        //        var chars = bnfRule.TerminalValue.ToCharArray();
        //        bool createdNewStateOnFirstStep = false;
        //        while(!createdNewStateOnFirstStep && hasFirstChar)
        //            for (var j = 0; j < chars.Length; j++)
        //            {
        //                var c = chars[j];
        //                Transition tr;
        //                if ((tr = currentState.Transitions.SingleOrDefault(t => t.Conditions.Contains(c))) != null)// jei is einamos busenos jau yra perejimas su duotu simboliu
        //                {
        //                    currentState = tr.To;
        //                    if (shouldBeFinal && j == chars.Length - 1 && !currentState.IsFinal) // jei paskutinis einamos alternatyvos simbolis
        //                    {
        //                        currentState.IsFinal = true;
        //                        currentState.LexemeType = lexemeType;
        //                    }
        //                }
        //                else
        //                {
        //                    if (j == 0)
        //                        createdNewStateOnFirstStep = true;
        //                    var newState = new State();
        //                    if (shouldBeFinal && j == chars.Length - 1) // jei paskutinis einamos alternatyvos simbolis
        //                    {
        //                        newState.IsFinal = true;
        //                        newState.LexemeType = lexemeType;
        //                    }
        //                    var newTransition = new Transition
        //                    {
        //                        To = newState,
        //                        From = currentState,
        //                        Conditions = new List<char> { c }
        //                    };
        //                    currentState.Transitions.Add(newTransition);
        //                    currentState = newState;
        //                }
        //            }
        //        newCurrentStates.Add(currentState);
        //    }

        //    return newCurrentStates;
        //}
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
