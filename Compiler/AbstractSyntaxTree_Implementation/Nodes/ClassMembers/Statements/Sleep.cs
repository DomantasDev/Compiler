using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions;
using AbstractSyntaxTree_Implementation.Nodes.Types;
using AbstractSyntaxTree_Implementation.ResolveNames;
using CodeGeneration.CodeGeneration;
using Common;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements
{
    public class Sleep : Statement
    {
        public Expression Expression { get; set; }

        public override void ResolveNames(Scope scope)
        {
            Expression.ResolveNames(scope);
        }

        public override Type CheckTypes()
        {
            var type = Expression.CheckTypes();

            if (type is ReferenceType || type.Value != "int")
            {
                $"Expected integer in sleep statement, got {type.Value}".RaiseError(type.Line);
            }

            return null;
        }

        public override void GenerateCode(CodeWriter w)
        {
            Expression.GenerateCode(w);
            w.Write(Instr.I_SLEEP);
        }
    }
}
