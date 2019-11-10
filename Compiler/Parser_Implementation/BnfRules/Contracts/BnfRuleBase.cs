using System.Collections.Generic;

namespace Parser_Implementation.BnfRules.Contracts
{
    public abstract class BnfRuleBase : IBnfRule
    {
        protected readonly List<IBnfRule> BnfRules;

        protected BnfRuleBase(List<IBnfRule> bnfRules, string ruleName)
        {
            RuleName = ruleName;
            BnfRules = bnfRules;
        }
        public abstract bool Expect();
        public string RuleName { get; }
    }
}
