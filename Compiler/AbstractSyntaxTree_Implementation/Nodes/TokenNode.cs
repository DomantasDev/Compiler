using System;
using System.Collections.Generic;
using System.Text;
using Lexer_Implementation.DynamicLexer;

namespace AbstractSyntaxTree_Implementation.Nodes
{
    public class TokenNode : Node, ITokenNode
    {
        public Token Token { get; set; }
    }
}
