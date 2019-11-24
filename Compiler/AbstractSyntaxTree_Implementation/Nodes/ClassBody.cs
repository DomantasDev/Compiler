using System.Collections.Generic;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers;

namespace AbstractSyntaxTree_Implementation.Nodes
{
    public class ClassBody : Node
    {
        public List<ClassMember> Members { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print("Members", Members);
        }
    }
}