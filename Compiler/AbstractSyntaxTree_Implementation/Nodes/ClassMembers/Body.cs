using System.Collections.Generic;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements;
using AbstractSyntaxTree_Implementation.Nodes.Types;
using AbstractSyntaxTree_Implementation.ResolveNames;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers
{
    public class Body : Node
    {
        public List<Statement> Statements { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(Statements), Statements);
        }

        public override void ResolveNames(Scope scope)
        {
            Statements.ForEach(x => x.ResolveNames(scope));
        }

        public override Type CheckTypes()
        {
            Statements.ForEach(x => x.CheckTypes());

            return null;
        }
    }
}