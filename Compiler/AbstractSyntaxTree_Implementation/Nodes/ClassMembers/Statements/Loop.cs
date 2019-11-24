using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements
{
    public class Loop : Statement
    {
        public Expression Condition { get; set; }
        public Body Body { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(Condition), Condition);
            p.Print(nameof(Body), Body);
        }
    }
}
