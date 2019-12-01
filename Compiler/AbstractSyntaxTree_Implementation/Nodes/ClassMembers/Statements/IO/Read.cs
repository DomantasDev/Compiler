using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.ResolveNames;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements.IO
{
    public class Read : Statement
    {
        public List<TokenNode> Variables { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(Variables), Variables);
        }

        public override void ResolveNames(Scope scope)
        {
            Variables.ForEach(x => x.ResolveNames(scope));
        }

        public override Type CheckTypes()
        {
            return null;
        }
    }
}
