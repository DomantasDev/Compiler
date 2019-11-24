using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions
{
    public class Cast : Expression
    {
        public Type Type { get; set; }
        public Expression Expression { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(Type), Type);
            p.Print(nameof(Expression), Expression);
        }
    }
}
