using System;
using System.Collections.Generic;
using System.Text;
using Parser_Implementation.BnfRules.Contracts;
using Parser_Implementation.Lexemes;

namespace Parser_Implementation.BnfRules.Alternatives
{
    public class BnfRuleAlternatives : BnfRuleBase
    {
        public BnfRuleAlternatives(List<IBnfRule> bnfRules, string ruleName, LexemeSource lexemeSource) : base(bnfRules, ruleName, lexemeSource)
        {
        }

        public override bool Expect()
        {
            foreach (var bnfRule in BnfRules)
            {
                var checkpoint = LexemeSource.SetCheckpoint();

                if (bnfRule.Expect())
                    return true;

                LexemeSource.RevertCheckPoint(checkpoint);
            }

            return false;
        }
    }
}
