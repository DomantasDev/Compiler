using System;
using System.Collections.Generic;
using System.Text;
using Lexer_Contracts;

namespace AbstractSyntaxTree_Implementation.Nodes
{
    public interface ITokenNode
    {
        string Type { get; set; }
        string Value { get; set; }
        int Line { get; set; }
    }
}
