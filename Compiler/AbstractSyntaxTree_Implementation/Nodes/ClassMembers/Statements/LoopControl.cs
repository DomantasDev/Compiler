using System;
using System.Collections.Generic;
using System.Text;
using Lexer_Implementation.DynamicLexer;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements
{
    public class LoopControl : Statement, ITokenNode
    {
        public Token Token { get; set; }
    }
}
