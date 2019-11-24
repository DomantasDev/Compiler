using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements.IO
{
    public class Write : Statement
    {
        public List<Expression> Arguments { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(Arguments), Arguments);
        }
    }
}
