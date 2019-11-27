using System;
using System.Collections.Generic;
using System.Text;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers
{
    public class Method : ClassMember
    {
        public Visibility Visibility { get; set; }
        public TokenNode Virtual_Override { get; set; }
        public Type ReturnType { get; set; }
        public TokenNode Name { get; set; }
        public List<Parameter> Parameters { get; set; }
        public Body Body { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(Visibility), Visibility);
            p.Print(nameof(Virtual_Override), Virtual_Override);
            p.Print(nameof(ReturnType), ReturnType);
            p.Print(nameof(Name), Name);
            p.Print(nameof(Parameters), Parameters);
            p.Print(nameof(Body), Body);
        }
    }
}
