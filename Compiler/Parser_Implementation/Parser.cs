using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Lexer_Implementation.DynamicLexer;
using Parser_Implementation.BnfRules.Contracts;
using Parser_Implementation.Lexemes;

namespace Parser_Implementation
{
    public class Parser
    {
        private readonly IBnfRule _rootRule;

        public Parser()
        {
            var dynamicLexer = new DynamicLexer("../../../DynamicParser/inception.bnf");

            var lexemeSource = new LexemeSource(new DynamicLexer("../../../DynamicLexer/lexemes.bnf")
                .GetLexemes(File.ReadAllText("../../../DynamicLexer/code.txt")).ToList());

            var bnfReader = new BnfReader.BnfReader(dynamicLexer, lexemeSource);
            _rootRule = bnfReader.GetRootRule("../../../DynamicParser/test_syntax.bnf");
        }

        public bool CheckSyntax()
        {
            return _rootRule.Expect();
        }
    }
}
