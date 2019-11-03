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

                var newRootRule = new BNFRule
                {
                    IsTerminal = false,
                    Alternatives = new List<List<BNFRule>>()
                };

                List<BNFRule> currentRules;
                if (rule.StartsWith('*'))
                {
                    currentRules = helpers;
                    rule = rule.Substring(1);
                }
                else if(rule.StartsWith('+'))
                {
                    currentRules = helpers;
                    rule = rule.Substring(1);
                    newRootRule.IsAtom = true;
                }
                else
                    currentRules = rules;

                var x = rule.Split("::=");
                var name = x[0].Trim();

                newRootRule.Name = name.Substring(1, name.Length - 2);

                var alternatives = x[1].Split(" | ");
                foreach (var alternative in alternatives)
                {
                    var newRules = new List<BNFRule>();
                    var altRules = alternative.Trim().Split(' '); // neleidzia turet tarpo simbolio. (\"[^\"]*\")|(<[^\"]*>)

                    foreach (var r in altRules)
                    {
                        var altRule = r.Trim();
                        BNFRule newRule;
                        if (altRule.StartsWith('\"') && altRule.EndsWith('\"'))
                        {
                            newRule = new BNFRule
                            {
                                IsTerminal = true,
                                TerminalValue = ReplaceWithEscapeChars(altRule.Substring(1, altRule.Length - 2))
                            };
                        }
                        else
                        {
                            newRule = new BNFRule
                            {
                                IsTerminal = false,
                                Name = ReplaceWithEscapeChars(altRule.Substring(1, altRule.Length - 2))
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

        private readonly Dictionary<char, char>  _escapeChars = new Dictionary<char, char>
        {
            {'\\', '\\'}, //   \\ -> \
            {'n', '\n'},  //   \n -> new line
            {'t', '\t'},  //    \t -> tab
            {'r','\r'},  //    \r -> car. ret.
            {'\"', '\"'},  //   \" -> "
        };
        private string ReplaceWithEscapeChars(string s)
        {
            var res = string.Empty;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '\\')
                {
                    res += _escapeChars[s[++i]];
                }
                else
                    res += s[i];
            }
            return res;
        }
    }
}
