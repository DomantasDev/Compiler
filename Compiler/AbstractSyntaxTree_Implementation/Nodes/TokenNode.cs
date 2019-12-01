using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AbstractSyntaxTree_Implementation.ResolveNames;
using Lexer_Contracts;

namespace AbstractSyntaxTree_Implementation.Nodes
{
    public class TokenNode : Node, ITokenNode
    {
        public Node Target { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public int Line { get; set; }

        public override void ResolveNames(Scope scope)
        {
            Target = scope.ResolveName(new Name(this, NameType.Variable));
        }
    }
}
