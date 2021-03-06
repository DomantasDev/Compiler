﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using Lexer_Contracts;
using Lexer_Implementation.DynamicLexer;
using Parser_Implementation.BnfRules;
using Parser_Implementation.BnfRules.Alternatives;
using Parser_Implementation.BnfRules.Contracts;
using Parser_Implementation.Lexemes;

namespace Parser_Implementation.BnfReader
{
    public class BnfReader
    {
        private readonly DynamicLexer _lexer;
        private readonly LexemeSource _lexemeSource;
        private readonly Dictionary<string, List<Token>> _lexemeDic;
        private readonly Dictionary<string, IBnfRule> _rulesInProgress;
        private readonly List<(string ruleName, Action<IBnfRule> updateAction)> _pendingUpdates;

        public BnfReader(DynamicLexer lexerForBnf, LexemeSource lexemeSource)
        {
            _lexer = lexerForBnf;
            _lexemeSource = lexemeSource;
            _lexemeDic = new Dictionary<string, List<Token>>();
            _rulesInProgress = new Dictionary<string, IBnfRule>();
            _pendingUpdates = new List<(string ruleName, Action<IBnfRule> updateAction)>();
        }

        public IBnfRule GetRootRule(string pathToParserBnf)
        {
            var bnfLines = File.ReadAllLines(pathToParserBnf).Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
            var lexemes = _lexer.GetLexemes(bnfLines[0]).SkipLast(1).ToList();
            var rootRuleName = lexemes.First().Value;

            foreach (var bnfLine in bnfLines.Skip(1))
            {
                AddRuleToDictionary(lexemes);

                lexemes = _lexer.GetLexemes(bnfLine).SkipLast(1).ToList();
            }
            AddRuleToDictionary(lexemes);

            var rootRules = new List<IBnfRule>
            {
                GetRule(rootRuleName),
                GetLexeme(new Token
                {
                    Type = "LEXEME_RULE",
                    Value = "*<EOF>"
                })
            };
            var root = new BnfRuleAlternative(rootRules, _lexemeSource, null, null);

            UpdateRules();

            return root;
        }


        private IBnfRule GetRule(string ruleName, Action<IBnfRule> updateAction = null)
        {
            if (_rulesInProgress.ContainsKey(ruleName))
            {
                if (updateAction != null)
                {
                    _pendingUpdates.Add((ruleName, updateAction));
                    return null;
                }

                throw new Exception("Rule already in progress but no update action specified");
            }


            _rulesInProgress.Add(ruleName, null);
            var newRule = GetRule(_lexemeDic[ruleName]);
            _rulesInProgress[ruleName] = newRule;
            return newRule;
        }

        private void UpdateRules()
        {
            foreach (var pendingUpdate in _pendingUpdates)
            {
                var rule = _rulesInProgress[pendingUpdate.ruleName] ?? throw new Exception("nu tep neturi but");
                pendingUpdate.updateAction(rule);
            }
        }
        private IBnfRule GetRule(List<Token> lexemes)
        {
            var alternatives = SplitToAlternatives(lexemes);
            if (alternatives.Count > 1)
                return GetAlternatives(alternatives);

            return GetSingleAlternative(alternatives.Single());
        }

        private IBnfRule GetSingleAlternative(List<Token> lexemes)
        {
            var repetitionSymbolsEncountered = 0;

            MetaData metaData;
            (lexemes, metaData) = lexemes.ExtractMetaData();
            var rules = new List<IBnfRule>();

            var repetitions = new List<Repetition>();
            var newRepetition = new Repetition();

            for (var i = 0; i < lexemes.Count; i++)
            {
                var temp = i - repetitionSymbolsEncountered;
                switch (lexemes[i].Type)
                {
                    case "LEXEME_RULE":
                        rules.Add(GetLexeme(lexemes[i]));
                        break;
                    case "RULE":
                        rules.Add(GetRule(lexemes[i].Value, rule => rules[temp] = rule));
                        break;
                    case "OP_REP_START":
                        newRepetition.StartIndex = i;
                        repetitionSymbolsEncountered++;
                        break;
                    case "OP_REP_END":
                        repetitionSymbolsEncountered++;
                        newRepetition.EndIndex = i - 2;
                        repetitions.Add(newRepetition);
                        newRepetition = new Repetition();
                        break;
                    default:
                        throw new Exception("dis iz no gud");
                }
            }

            return new BnfRuleAlternative(rules, _lexemeSource, repetitions, metaData);
        }

        //private IBnfRule GetRepetition(List<Lexeme> lexemes)
        //{
        //    return new BnfRuleRepetition(GetRule(lexemes), _lexemeSource);
        //}

        private IBnfRule GetLexeme(Token lexeme)
        {
            var lexemeName = lexeme.Value.Substring(2, lexeme.Value.Length - 3);
            return new BnfRuleLexeme(_lexemeSource, lexemeName);
        }

        private IBnfRule GetAlternatives(List<List<Token>> alternatives)
        {
            return new BnfRuleAlternatives(alternatives.Select(a => GetSingleAlternative(a)).ToList(), _lexemeSource);
        }

        private List<List<Token>> SplitToAlternatives(List<Token> lexemes)
        {
            var insideRepetition = false;
            var alternatives = new List<List<Token>>();
            var list = new List<Token>();
            foreach (var lexeme in lexemes)
            {
                if (lexeme.Type == "OP_REP_START")
                    insideRepetition = true;
                else if (lexeme.Type == "OP_REP_END")
                    insideRepetition = false;
                
                if (lexeme.Type == "OP_ALT" && !insideRepetition)
                {
                    alternatives.Add(list);
                    list = new List<Token>();
                }
                else
                {
                    list.Add(lexeme);
                }
            }

            alternatives.Add(list);
            return alternatives;
        }

        private void AddRuleToDictionary(List<Token> lexemes)
        {
            if (lexemes[0].Type == "RULE")
                _lexemeDic.Add(lexemes[0].Value, lexemes.Skip(2).ToList());
            else
                throw new Exception("dis iz not gud");
        }
    }
}
