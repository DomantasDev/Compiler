using System;
using System.Collections.Generic;
using System.Text;
using Parser_Implementation.BnfRules.Contracts;
using Parser_Implementation.Lexemes;

namespace Parser_Implementation.BnfRules
{
    public class BnfRuleLexeme : IBnfRule
    {
        private readonly LexemeSource _lexemeSource;
        public string RuleName { get; }

        public BnfRuleLexeme(LexemeSource lexemeSource,string ruleName)
        {
            _lexemeSource = lexemeSource;
            RuleName = ruleName;
        }

        public bool Expect()
        {
            return _lexemeSource.Expect(RuleName);
        }
    }
}
