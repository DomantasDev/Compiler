using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements
{
    public class Assign : Statement
    {
        public TokenNode VariableName { get; set; }
        public Expression Expression { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(VariableName), VariableName);
            p.Print(nameof(Expression), Expression);
        }
    }
}
