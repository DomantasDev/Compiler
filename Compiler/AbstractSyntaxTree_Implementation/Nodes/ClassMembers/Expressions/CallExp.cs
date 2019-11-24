using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions
{
    public class CallExp : Expression
    {
        public TokenNode MethodName { get; set; }
        public List<Expression> Arguments { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(MethodName), MethodName);
            p.Print(nameof(Arguments), Arguments);
        }
    }
}
