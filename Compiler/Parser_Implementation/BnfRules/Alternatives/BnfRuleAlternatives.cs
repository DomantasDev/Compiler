using System;
using System.Collections.Generic;
using System.Text;
using Parser_Implementation.BnfRules.Contracts;
using Parser_Implementation.Lexemes;

namespace Parser_Implementation.BnfRules.Alternatives
{
    public class BnfRuleAlternatives : BnfRuleBase
    {
        private readonly LexemeSource _lexemeSource;

        public BnfRuleAlternatives(List<IBnfRule> bnfRules, string ruleName, LexemeSource lexemeSource) : base(bnfRules, ruleName)
        {
            _lexemeSource = lexemeSource;
        }

        public override bool Expect()
        {
            foreach (var bnfRule in BnfRules)
            {
                _lexemeSource.SetCheckpoint();
                if (bnfRule.Expect())
                    return true;
                _lexemeSource.RevertCheckPoint();
            }

            return false;
        }
    }
}
