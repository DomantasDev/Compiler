using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions;
using AbstractSyntaxTree_Implementation.ResolveNames;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements
{
    public class Assign : Statement
    {
        public TokenNode Variable { get; set; } // reikia targeto
        public Expression Expression { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(Variable), Variable);
            p.Print(nameof(Expression), Expression);
        }

        public override void ResolveNames(Scope scope)
        {
            Variable.ResolveNames(scope);
            Expression.ResolveNames(scope);
        }

        public override Type CheckTypes()
        {
            var type = (Type)Variable.Target?.GetType().GetProperty("Type")?.GetMethod.Invoke(Variable.Target, null);

            type?.IsCompatible(Expression.CheckTypes());

            return null;
        }
    }
}
