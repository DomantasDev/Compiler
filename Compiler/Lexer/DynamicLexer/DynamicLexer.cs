﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lexer_Contracts;
using Lexer_Implementation.DynamicLexer.BnfReader;
using Lexer_Implementation.DynamicLexer.FSM;

namespace Lexer_Implementation.DynamicLexer
{
    public class DynamicLexer
    {
        private readonly StateMachine _stateMachine;
        private readonly List<string> _skippableRules;
        public DynamicLexer(string pathToBNF)
        {
            var reader = new BNFReader(pathToBNF);
            var bnfData = reader.GetRules();
            _skippableRules = bnfData.SkippableRules;

            bnfData.HelperRules.Add(new BNFRule
            {
                Name = "SPACE",
                Alternatives = new List<List<BNFRule>> { new List<BNFRule> { new BNFRule
                {
                    IsTerminal = true,
                    TerminalValue = " "
                } } } 
            });
            LinkRules(bnfData.Rules.Union(bnfData.HelperRules).ToList());

            _stateMachine = new StateMachineBuilder().Build(bnfData.Rules);
        }

        public IEnumerable<Token> GetLexemes(string code, bool skip = true)
        {
            int line = 1;
            int globalOffset = 0;
            int iterationOffset = 0;
            for (; globalOffset + iterationOffset < code.Length; iterationOffset++)
            {
                var nextChar = code[globalOffset + iterationOffset];

                if (nextChar == '\n')
                    line++;

                if (!_stateMachine.Advance(nextChar) || globalOffset + iterationOffset == code.Length - 1)
                {
                    if (_stateMachine.LastFinalState == null)
                    {
                        if (string.IsNullOrWhiteSpace(_stateMachine.Path))
                        {
                            _stateMachine.Reset();
                            yield return new Token
                            {
                                Type = "EOF",
                                Value = "EOF"
                            };
                            yield break;
                        }
                        throw new ArgumentException($"Unrecognized sequence: {_stateMachine.Path}");
                    }
                    var lastState = _stateMachine.LastFinalState.Value;

                    globalOffset += lastState.value.Length;
                    iterationOffset = -1;

                    if (!skip || !_skippableRules.Contains(lastState.state.LexemeType))
                    {
                        yield return new Token
                        {
                            Value = lastState.value.Trim(),
                            Type = lastState.state.LexemeType,
                            Line = line
                        };
                    }
                        
                    _stateMachine.Reset();
                }
            }
            yield return new Token
            {
                Type = "EOF",
                Value = "EOF"
            };
        }

        private void LinkRules(List<BNFRule> allRules)
        {
            foreach (var rootRule in allRules)
            {
                foreach (var rootRuleAlternative in rootRule.Alternatives)
                {
                    for (var i = 0; i < rootRuleAlternative.Count; i++)
                    {
                        if (!rootRuleAlternative[i].IsTerminal)
                        {
                            rootRuleAlternative[i] = GetRule(rootRuleAlternative[i].Name);
                        }
                    }
                }
            }

            BNFRule GetRule(string ruleName)
            {
                return allRules.Single(r => r.Name == ruleName);
            }
        }

    }
}
