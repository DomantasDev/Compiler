using System;
using System.Collections.Generic;
using System.Text;
using Lexer_Implementation.DynamicLexer;

namespace Parser_Implementation.BnfRules.Contracts
{
    public class MetaRuleBase
    {
        protected MetaData MetaData { get; set; }

        public MetaRuleBase(MetaData metaData)
        {
            MetaData = metaData;
        }
    }
}
