using System.Collections.Generic;
using Parser_Implementation.BnfRules.Contracts;
using Parser_Implementation.Lexemes;

namespace Parser_Implementation.BnfRules.Alternatives
{
    public class BnfRuleAlternative : BnfRuleBase
    {


        public BnfRuleAlternative(List<IBnfRule> bnfRules, string ruleName, LexemeSource lexemeSource) : base(bnfRules, ruleName, lexemeSource)
        {
        }

        public override bool Expect()
        {
            var checkpoint = LexemeSource.SetCheckpoint();
            foreach (var bnfRule in BnfRules)
            {
                if (!bnfRule.Expect())
                {
                    LexemeSource.RevertCheckPoint(checkpoint);
                    return false;
                }
            }

            return true;
        }
    }
}
