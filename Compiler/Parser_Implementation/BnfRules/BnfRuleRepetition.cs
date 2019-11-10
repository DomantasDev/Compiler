using System;
using System.Collections.Generic;
using System.Text;
using Parser_Implementation.BnfRules.Contracts;
using Parser_Implementation.Lexemes;

namespace Parser_Implementation.BnfRules
{
    public class BnfRuleRepetition : BnfRuleBase
    {
        private readonly LexemeSource _lexemeSource;
        public BnfRuleRepetition(List<IBnfRule> bnfRules, string ruleName) : base(bnfRules, ruleName) {}

        public BnfRuleRepetition(IBnfRule bnfRule, LexemeSource lexemeSource) : base(new List<IBnfRule>{bnfRule}, "Repetition")
        {
            _lexemeSource = lexemeSource;
        }

        public override bool Expect()
        {
            while (true)
            {
                _lexemeSource.SetCheckpoint();
                if (!BnfRules.Expect())
                {
                    _lexemeSource.RevertCheckPoint();
                    break;
                }
            }

            return true;
        }
    }
}
