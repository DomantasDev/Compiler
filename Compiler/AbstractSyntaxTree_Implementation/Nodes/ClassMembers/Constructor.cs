using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers
{
    public class Constructor : ClassMember
    {
        public TokenNode Visibility { get; set; }
        public List<Parameter> Parameters { get; set; }
        public List<Expression> BaseArguments { get; set; }
        public Body Body { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(Visibility), Visibility);
            p.Print(nameof(Parameters), Parameters);
            p.Print(nameof(BaseArguments), BaseArguments);
            p.Print(nameof(Body), Body);
        }
    }
}
