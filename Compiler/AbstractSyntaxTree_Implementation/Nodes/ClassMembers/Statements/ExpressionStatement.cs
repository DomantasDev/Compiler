using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions;
using AbstractSyntaxTree_Implementation.ResolveNames;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements
{
    public class ExpressionStatement : Statement
    {
        public Expression Expression { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(Expression), Expression);
        }

        public override void ResolveNames(Scope scope)
        {
            Expression.ResolveNames(scope);
        }

        public override Type CheckTypes()
        {
            Expression.CheckTypes();

            return null;
        }
    }
}
