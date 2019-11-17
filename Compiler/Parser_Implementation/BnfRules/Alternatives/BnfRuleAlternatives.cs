using System;
using System.Collections.Generic;
using System.Text;
using Parser_Implementation.BnfRules.Contracts;
using Parser_Implementation.Lexemes;

namespace Parser_Implementation.BnfRules.Alternatives
{
    public class BnfRuleAlternatives : BnfRuleBase
    {
        public BnfRuleAlternatives(List<IBnfRule> bnfRules, LexemeSource lexemeSource) : base(bnfRules, lexemeSource, null)
        {
        }

        public override ExpectResult Expect()
        {
            foreach (var bnfRule in BnfRules)
            {
                var checkpoint = LexemeSource.SetCheckpoint();

                var res = bnfRule.Expect();
                if (res.Success)
                    return res;

                LexemeSource.RevertCheckPoint(checkpoint);
            }

            return new ExpectResult(false);
        }
    }
}
