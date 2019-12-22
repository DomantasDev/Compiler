using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.CodeGeneration;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions;
using AbstractSyntaxTree_Implementation.Nodes.Types;
using AbstractSyntaxTree_Implementation.ResolveNames;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;
using ValueType = AbstractSyntaxTree_Implementation.Nodes.Types.ValueType;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements
{
    public class Return : Statement
    {
        //kw

        public Expression Expression { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(Expression), Expression);
        }

        public override void ResolveNames(Scope scope)
        {
            Expression?.ResolveNames(scope);
        }

        public override Type CheckTypes()
        {
            var retType = FindAncestor<Method>().ReturnType;
            retType.IsCompatible(Expression?.CheckTypes() ?? new ValueType { Value = "void"});

            return null;
        }

        public override void GenerateCode(CodeWriter w)
        {
            if (Expression != null)
            {
                Expression.GenerateCode(w);
                w.Write(Instr.I_RETV);
            }
            else
            {
                w.Write(Instr.I_RET);
            }
        }
    }
}
