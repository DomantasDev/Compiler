using System;
using System.Collections.Generic;
using System.Text;
using CodeGeneration.CodeGeneration;
using Common;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions.Unary
{
    public class UnaryLogicExp : UnaryExp
    {
        public override Type CheckTypes()
        {
            Type = Expression.CheckTypes();
            if (Type.Value != "bool")
            {
                $"\"{Type.Value}\" cannot be used in logic expression".RaiseError(Operator.Line);
                return null;
            }

            return Type;
        }

        public override void GenerateCode(CodeWriter w)
        {
            Expression.GenerateCode(w);
            w.Write(Instr.I_NOT);
        }
    }
}
