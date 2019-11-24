using System;
using System.Collections.Generic;
using System.Text;
using Lexer_Implementation.DynamicLexer;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions
{
    public class LiteralExp : Expression, ITokenNode
    {
        public Token Token { get; set; }
    }
}
