using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions;
using AbstractSyntaxTree_Implementation.Nodes.Types;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers
{
    public class VariableDeclaration : ClassMember
    {
        public Visibility Visibility { get; set; }
        public Type Type { get; set; }
        public TokenNode Name { get; set; }
        public Expression Expression { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print("visibility", Visibility);
            p.Print("type", Type);
            p.Print("name", Name);
            p.Print("expression", Expression);
        }
    }
}
