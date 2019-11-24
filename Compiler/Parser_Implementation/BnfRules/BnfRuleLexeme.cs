using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes;
using Parser_Implementation.BnfRules.Contracts;
using Parser_Implementation.Lexemes;

namespace Parser_Implementation.BnfRules
{
    public class BnfRuleLexeme : IBnfRule
    {
        private readonly LexemeSource _lexemeSource;
        public string RuleName { get; }

        public BnfRuleLexeme(LexemeSource lexemeSource, string ruleName)
        {
            _lexemeSource = lexemeSource;
            RuleName = ruleName;
        }

        public ExpectResult Expect()
        {
            var res =_lexemeSource.Expect(RuleName);
            if(!res.Success)
                return  new ExpectResult();

            return new ExpectResult
            {
                Node = new TokenNode
                {
                    Token = res.Lexeme
                },
                Success = true
            };
        }
    }
}
