using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.ResolveNames;
using Common;
using Lexer_Contracts;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements
{
    public class LoopControl : Statement, ITokenNode
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public int Line { get; set; }

        public override void ResolveNames(Scope scope)
        {
            var ancestor = FindAncestor<Loop>();
            if(ancestor == null)
                $"{Value} not in a loop".RaiseError(Line);
        }

        public override Type CheckTypes()
        {
            return null;
        }
    }
}
