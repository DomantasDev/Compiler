using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lexer_Implementation.DynamicLexer
{
    public class DynamicLexer
    {
        public DynamicLexer(string pathToBNF)
        {
            var reader = new BNFReader(pathToBNF);
            var (rules, helpers) = reader.GetRules();

            LinkRules(rules, helpers);


        }

        private void LinkRules(List<BNFRule> rules, List<BNFRule> helpers)
        {
            foreach (var rootRule in rules)
            {
                foreach (var rootRuleAlternative in rootRule.Alternatives)
                {
                    for (var i = 0; i < rootRuleAlternative.Count; i++)
                    {
                        if (!rootRuleAlternative[i].IsTerminal)
                        {
                            rootRuleAlternative[i] = GetRule(rootRuleAlternative[i].Name, rules.Union(helpers));
                        }
                    }
                }
            }
        }

        private BNFRule GetRule(string ruleName, IEnumerable<BNFRule> rules)
        {
            return rules.Single(r => r.Name == ruleName);
        }
    }
}
