using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions;
using AbstractSyntaxTree_Implementation.Nodes.Types;
using AbstractSyntaxTree_Implementation.ResolveNames;
using CodeGeneration.CodeGeneration;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers
{
    public class VariableDeclaration : ClassMember
    {
        public Visibility Visibility { get; set; }
        public Type Type { get; set; }
        public Expression Expression { get; set; }

        public int HeapSlot { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print("visibility", Visibility);
            p.Print("type", Type);
            p.Print("name", Name);
            p.Print("expression", Expression);
        }

        public override void AddName(Scope scope)
        {
            scope.Add(new Name(Name, NameType.Variable), this);
        }

        public override void ResolveNames(Scope scope)
        {
            Type.ResolveNames(scope);
            Expression?.ResolveNames(scope);
        }

        public override Type CheckTypes()
        {
            Type.IsCompatible(Expression?.CheckTypes());

            return null;
        }

        public override void GenerateCode(CodeWriter w)
        {
            //TODO remove expression
        }
    }
}
