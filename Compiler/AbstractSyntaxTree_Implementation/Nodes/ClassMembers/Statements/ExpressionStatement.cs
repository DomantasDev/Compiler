﻿using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions;
using AbstractSyntaxTree_Implementation.ResolveNames;
using CodeGeneration.CodeGeneration;
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

        public override void GenerateCode(CodeWriter w)
        {
            Expression.GenerateCode(w);
            //TODO POP
        }
    }
}
