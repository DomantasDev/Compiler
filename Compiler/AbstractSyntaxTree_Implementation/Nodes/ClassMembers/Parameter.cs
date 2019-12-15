using System.Threading;
using AbstractSyntaxTree_Implementation.CodeGeneration;
using AbstractSyntaxTree_Implementation.Nodes.Types;
using AbstractSyntaxTree_Implementation.ResolveNames;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers
{
    public class Parameter : Node, ITargetable
    {
        public Type Type { get; set; }
        public TokenNode Name { get; set; }

        public int StackSlot { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(Type), Type);
            p.Print(nameof(Name), Name);
        }

        public override void ResolveNames(Scope scope)
        {
            StackSlot = ++Method.LocalVariableCount;
            Type.ResolveNames(scope);
            scope.Add(new Name(Name, NameType.Variable), this);
        }

        public Label StartLabel { get; } = new Label();
    }
}