using System.Collections.Generic;
using Parser_Implementation.BnfRules.Contracts;

namespace Parser_Implementation.BnfRules.Alternatives
{
    public class BnfRuleAlternative : BnfRuleBase
    {
        

        public BnfRuleAlternative(List<IBnfRule> bnfRules, string ruleName) : base(bnfRules, ruleName)
        {
        }

        public override bool Expect()
        {
            foreach (var bnfRule in BnfRules)
            {
                if (!bnfRule.Expect())
                    return false;
            }

            return true;
        }
    }
}
