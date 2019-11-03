using System;
using System.Collections.Generic;

namespace Lexer_Implementation.DynamicLexer
{
    public class BNFRule
    {
        public string Name { get; set; }
        public List<List<BNFRule>> Alternatives { get; set; }

        public bool IsTerminal { get; set; }

        public string TerminalValue { get; set; }

        internal bool IsAtom { get; set; }
    }
}