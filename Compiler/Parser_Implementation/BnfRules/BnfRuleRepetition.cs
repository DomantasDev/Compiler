using System;
using System.Collections.Generic;
using System.Text;
using Parser_Implementation.BnfRules.Contracts;
using Parser_Implementation.Lexemes;

namespace Parser_Implementation.BnfRules
{
    //public class BnfRuleRepetition : BnfRuleBase
    //{
    //    public BnfRuleRepetition(IBnfRule bnfRule, LexemeSource lexemeSource) : base(new List<IBnfRule>{bnfRule}, "Repetition", lexemeSource, )
    //    {
    //    }

    //    public override ExpectResult Expect()
    //    {
    //        while (true)
    //        {
    //            var checkpoint = LexemeSource.SetCheckpoint();
    //            if (!BnfRules.Expect())
    //            {
    //                LexemeSource.RevertCheckPoint(checkpoint);
    //                break;
    //            }
    //        }

    //        return true;
    //    }
    //}
}
