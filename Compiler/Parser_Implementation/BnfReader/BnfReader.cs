using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
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
        private readonly Dictionary<string, List<Lexeme>> _lexemeDic;
        private readonly Dictionary<string, IBnfRule> _rulesInProgress;
        private readonly List<(string ruleName, Action<IBnfRule> updateAction)> _pendingUpdates;

        public BnfReader(DynamicLexer lexerForBnf, LexemeSource lexemeSource)
        {
            _lexer = lexerForBnf;
            _lexemeSource = lexemeSource;
            _lexemeDic = new Dictionary<string, List<Lexeme>>();
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
                GetLexeme(new Lexeme
                {
                    Type = "LEXEME_RULE",
                    Value = "*<EOF>"
                })
            };
            var root = new BnfRuleAlternative(rootRules, "ROOT", _lexemeSource);

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
        private IBnfRule GetRule(List<Lexeme> lexemes)
        {
            var alternatives = SplitToAlternatives(lexemes);
            if (alternatives.Count > 1)
                return GetAlternatives(alternatives);

            return GetSingleAlternative(alternatives.Single());
        }

        private IBnfRule GetSingleAlternative(List<Lexeme> lexemes)
        {
            var rules = new List<IBnfRule>();
            for (var i = 0; i < lexemes.Count; i++)
            {
                var temp = i;
                switch (lexemes[i].Type)
                {
                    case "LEXEME_RULE":
                        rules.Add(GetLexeme(lexemes[i]));
                        break;
                    case "RULE":
                        rules.Add(GetRule(lexemes[i].Value, rule => rules[temp] = rule));
                        break;
                    case "OP_REP_START":
                    {
                        var repetitionLexemes = new List<Lexeme>(); 
                        int j;

                        for (j = i + 1;; j++)
                        {
                            if (lexemes[j].Type == "OP_REP_END")
                                break;
                            repetitionLexemes.Add(lexemes[j]);
                        }

                        i = j;
                        rules.Add(GetRepetition(repetitionLexemes));
                        break;
                    }
                    default:
                        throw new Exception("dis iz no gud");
                }
            }

            return new BnfRuleAlternative(rules, "SingleAlternative", _lexemeSource);
        }

        private IBnfRule GetRepetition(List<Lexeme> lexemes)
        {
            return new BnfRuleRepetition(GetRule(lexemes), _lexemeSource);
        }

        private IBnfRule GetLexeme(Lexeme lexeme)
        {
            var lexemeName = lexeme.Value.Substring(2, lexeme.Value.Length - 3);
            return new BnfRuleLexeme(_lexemeSource, lexemeName);
        }

        private IBnfRule GetAlternatives(List<List<Lexeme>> alternatives)
        {
            return new BnfRuleAlternatives(alternatives.Select(a => GetSingleAlternative(a)).ToList(), "Alternatives", _lexemeSource);
        }

        private List<List<Lexeme>> SplitToAlternatives(List<Lexeme> lexemes)
        {
            var insideRepetition = false;
            var alternatives = new List<List<Lexeme>>();
            var list = new List<Lexeme>();
            foreach (var lexeme in lexemes)
            {
                if (lexeme.Type == "OP_REP_START")
                    insideRepetition = true;
                else if (lexeme.Type == "OP_REP_END")
                    insideRepetition = false;
                
                if (lexeme.Type == "OP_ALT" && !insideRepetition)
                {
                    alternatives.Add(list);
                    list = new List<Lexeme>();
                }
                else
                {
                    list.Add(lexeme);
                }
            }

            alternatives.Add(list);
            return alternatives;
        }

        private void AddRuleToDictionary(List<Lexeme> lexemes)
        {
            if (lexemes[0].Type == "RULE")
                _lexemeDic.Add(lexemes[0].Value, lexemes.Skip(2).ToList());
            else
                throw new Exception("dis iz not gud");
        }
    }
}
