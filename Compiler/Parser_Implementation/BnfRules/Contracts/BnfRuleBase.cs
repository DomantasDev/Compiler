using System.Collections.Generic;
using Parser_Implementation.Lexemes;

namespace Parser_Implementation.BnfRules.Contracts
{
    public abstract class BnfRuleBase : IBnfRule
    {
        protected readonly List<IBnfRule> BnfRules;

        protected BnfRuleBase(List<IBnfRule> bnfRules, string ruleName, LexemeSource lexemeSource)
        {
            RuleName = ruleName;
            LexemeSource = lexemeSource;
            BnfRules = bnfRules;
        }
        public abstract bool Expect();
        public string RuleName { get; }
        protected LexemeSource LexemeSource { get; }
    }
}
