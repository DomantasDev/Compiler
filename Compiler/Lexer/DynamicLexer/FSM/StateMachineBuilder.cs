using System;
using System.Collections.Generic;
using System.Text;

namespace Lexer_Implementation.DynamicLexer.FSM
{
    public class StateMachineBuilder
    {
        public StateMachine Build(List<BNFRule> rules, List<BNFRule> helpers)
        {
            var start = new State();
            foreach (var bnfRule in rules)
            {
                foreach (var bnfRuleAlternative in bnfRule.Alternatives)
                {
                    bnfRuleAlternative.fo
                }
            }
        }
    }
}
