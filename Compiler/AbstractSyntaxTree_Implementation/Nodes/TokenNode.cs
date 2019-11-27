using System;
using System.Collections.Generic;
using System.Text;
using Lexer_Contracts;

namespace AbstractSyntaxTree_Implementation.Nodes
{
    public class TokenNode : Node, ITokenNode
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public int Line { get; set; }
    }
}
