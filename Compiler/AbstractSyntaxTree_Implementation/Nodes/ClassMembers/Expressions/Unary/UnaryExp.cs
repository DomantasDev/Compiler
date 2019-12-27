using AbstractSyntaxTree_Implementation.ResolveNames;
using CodeGeneration.CodeGeneration;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions.Unary
{
    public abstract class UnaryExp : Expression
    {
        public TokenNode Operator { get; set; }
        public Expression Expression { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(Operator), Operator);
            p.Print(nameof(Expression), Expression);
        }

        public override void ResolveNames(Scope scope)
        {
            Expression.ResolveNames(scope);
        }
    }
}
