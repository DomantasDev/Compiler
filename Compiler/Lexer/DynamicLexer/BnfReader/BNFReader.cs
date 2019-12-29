using System.Collections.Generic;
using System.IO;

namespace Lexer_Implementation.DynamicLexer.BnfReader
{
    public class BNFReader
    {
        private readonly string _pathToBnf;

        public BNFReader(string pathToBNF)
        {
            _pathToBnf = pathToBNF;
        }

        public BnfData GetRules()
        {
            var bnf = File.ReadAllText(_pathToBnf).Split('\n');

            //var bnfRules = bnf.TakeWhile(r => r.Trim() != "#helpers").ToArray();
            //var bnfHelpers = bnf.Skip(bnfRules.Length + 1).ToArray();

            //return (rules: GetRules(bnfRules), helperRules: GetRules(bnfHelpers));
            return GetRules(bnf);
        }

        private BnfData GetRules(string[] BNFRules)
        {
            var rules = new List<BNFRule>();
            var helpers = new List<BNFRule>();
            var skippableRules = new List<string>();
            foreach (var rule in BNFRules)
            {
                var newRootRule = new BNFRule
                {
                    IsTerminal = false,
                    Alternatives = new List<List<BNFRule>>()
                };

                var x = rule.Split("::=");
                var name = x[0].Trim();

                List<BNFRule> currentRules;
                if (name.StartsWith('*'))
                {
                    currentRules = helpers;
                    name = name.Substring(1);
                }
                else if (name.StartsWith('+'))
                {
                    currentRules = helpers;
                    name = name.Substring(1);
                    newRootRule.IsAtom = true;
                }
                else if (name.StartsWith('s'))
                {
                    currentRules = rules;
                    name = name.Substring(1);
                    skippableRules.Add(name.Substring(1, name.Length - 2));
                }
                else
                    currentRules = rules;

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

            return new BnfData
            {
                Rules = rules,
                HelperRules = helpers,
                SkippableRules = skippableRules
            };
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
