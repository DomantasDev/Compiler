using System;
using System.Collections.Generic;
using System.Text;

namespace Lexer_Implementation.DynamicLexer.BnfReader
{
    public class BnfData
    {
        public List<BNFRule> Rules { get; set; }
        public List<BNFRule> HelperRules { get; set; }
        public List<string> SkippableRules { get; set; }
    }
    
}
