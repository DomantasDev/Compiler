using System.Collections.Generic;
using AbstractSyntaxTree_Implementation;
using Parser_Implementation.Lexemes;

namespace Parser_Implementation.BnfRules.Contracts
{
    public abstract class BnfRuleBase : MetaRuleBase, IBnfRule
    {
        protected readonly List<IBnfRule> BnfRules;
        protected readonly NodeFactory NodeFactory;

        protected BnfRuleBase(List<IBnfRule> bnfRules, LexemeSource lexemeSource, MetaData metaData) : base(metaData)
        {
            LexemeSource = lexemeSource;
            NodeFactory = new NodeFactory();
            BnfRules = bnfRules;
        }
        public abstract ExpectResult Expect();

        protected LexemeSource LexemeSource { get; }
    }
}
