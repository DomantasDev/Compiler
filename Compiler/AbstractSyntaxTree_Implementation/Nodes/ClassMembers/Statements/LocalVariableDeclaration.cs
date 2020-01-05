using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions;
using AbstractSyntaxTree_Implementation.ResolveNames;
using CodeGeneration.CodeGeneration;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements
{
    public class LocalVariableDeclaration : Statement
    {
        public Type Type { get; set; }
        public TokenNode Name { get; set; }
        public Expression Expression { get; set; }
        public int StackSlot { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(Type), Type);
            p.Print(nameof(Name), Name);
            p.Print(nameof(Expression), Expression);
        }

        public override void ResolveNames(Scope scope)
        {
            StackSlot = Method.LocalVariableCount++;
            Type.ResolveNames(scope);
            scope.Add(new Name(Name, NameType.Variable), this);
            Expression?.ResolveNames(scope);
        }

        public override Type CheckTypes()
        {
            if(Expression != null)
                Type.IsCompatible(Expression.CheckTypes());

            return Type;
        }

        public override void GenerateCode(CodeWriter w)
        {
            if (Expression != null)
            {
                Expression.GenerateCode(w);
                w.Write(Instr.I_SET_L, StackSlot);
            }
        }
    }
}
