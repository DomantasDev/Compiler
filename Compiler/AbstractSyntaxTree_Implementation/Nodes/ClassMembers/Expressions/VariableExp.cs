using System;
using System.Collections.Generic;
using System.Text;
using Lexer_Contracts;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions
{
    public class VariableExp : Expression, ITokenNode
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public int Line { get; set; }
    }
}
