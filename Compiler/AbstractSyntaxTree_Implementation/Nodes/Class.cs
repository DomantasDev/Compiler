using AbstractSyntaxTree_Implementation.Nodes.ClassMembers;
using AbstractSyntaxTree_Implementation.Nodes.Types;
using AbstractSyntaxTree_Implementation.ResolveNames;
using CodeGeneration.CodeGeneration;

namespace AbstractSyntaxTree_Implementation.Nodes
{
    public class Class : Node
    {
        public TokenNode Name { get; set; }
        public ReferenceType Extends { get; set; }
        public ClassBody Body { get; set; }

        public Label VTableLabel { get; } = new Label();
        public Slots<Method> VTableSlots { get; set; }
        public Slots<VariableDeclaration> HeapSlots { get; set; }


        public Scope Scope { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print("name", Name);
            p.Print("extends", Extends);
            p.Print("Body", Body);
        }

        public override void ResolveNames(Scope scope)
        {
            Scope = scope;
            Extends?.ResolveNames(scope);
            Body?.ResolveNames(new Scope(scope));
        }

        public void AddNames(Scope scope)
        {
            Body?.AddNames(scope);
        }

        public override Type CheckTypes() // TODO check for duplicates in base class
        {
            Body?.CheckTypes();

            return null;
        }

        public override void GenerateCode(CodeWriter w)
        {
            Body.GenerateCode(w);
        }
    }
}