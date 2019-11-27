using System;
using System.Collections.Generic;
using System.Text;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions
{
    public class ObjCreationExp : Expression
    {
        public Type Type { get; set; }
        public List<Expression> Arguments { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(Type), Type);
            p.Print(nameof(Arguments), Arguments);
        }
    }
}
