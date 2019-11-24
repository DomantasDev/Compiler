using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements.If
{
    public class Else : Node
    {
        public If If { get; set; }
        public Body Body { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(If), If);
            p.Print(nameof(Body), Body);
        }
    }
}
