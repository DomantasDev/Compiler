using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lexer_Implementation.DynamicLexer
{
    public class BNFReader
    {
        private readonly string _pathToBnf;

        public BNFReader(string pathToBNF)
        {
            _pathToBnf = pathToBNF;
        }

        public (List<BNFRule> rules, List<BNFRule> helperRules) GetRules()
        {
            var bnf = File.ReadAllText(_pathToBnf).Split('\n');

            //var bnfRules = bnf.TakeWhile(r => r.Trim() != "#helpers").ToArray();
            //var bnfHelpers = bnf.Skip(bnfRules.Length + 1).ToArray();

            //return (rules: GetRules(bnfRules), helperRules: GetRules(bnfHelpers));
            return GetRules(bnf);
        }

        private (List<BNFRule> rules, List<BNFRule> helperRules) GetRules(string[] BNFRules)
        {
            var rules = new List<BNFRule>();
            var helpers = new List<BNFRule>();
            foreach (var ru in BNFRules)
            {
                var rule = ru;
                List<BNFRule> currentRules;
                if (rule.StartsWith('*'))
                {
                    currentRules = helpers;
                    rule = rule.Substring(1);
                }
                else
                    currentRules = rules;

                var x = rule.Split("::=");
                var name = x[0].Trim();

                var newRootRule = new BNFRule
                {
                    IsTerminal = false,
                    Name = name.Substring(1, name.Length - 2),
                    Alternatives = new List<List<BNFRule>>()
                };

                var alternatives = x[1].Split('|');
                foreach (var alternative in alternatives)
                {
                    var newRules = new List<BNFRule>();
                    var altRules = alternative.Trim().Split(' ');

                    foreach (var r in altRules)
                    {
                        var altRule = r.Trim();
                        BNFRule newRule;
                        if (altRule.StartsWith('\"') && altRule.EndsWith('\"'))
                        {
                            newRule = new BNFRule
                            {
                                IsTerminal = true,
                                TerminalValue = altRule.Substring(1, altRule.Length - 2)
                            };
                        }
                        else
                        {
                            newRule = new BNFRule
                            {
                                IsTerminal = false,
                                Name = altRule.Substring(1, altRule.Length - 2)
                            };
                        }
                        newRules.Add(newRule);
                    }
                    newRootRule.Alternatives.Add(newRules);
                }
                currentRules.Add(newRootRule);
            }

            return (rules, helpers);
        }
    }
}
