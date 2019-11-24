using System.Collections.Generic;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers
{
    public class Body : Node
    {
        public List<Statement> Statements { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(Statements), Statements);

        }
    }
}