using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes.Types;
using CodeGeneration.CodeGeneration;
using Common;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;
using ValueType = AbstractSyntaxTree_Implementation.Nodes.Types.ValueType;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions.Unary
{
    public class UnaryArithExp : UnaryExp
    {
        public override Type CheckTypes()
        {
            Type = Expression.CheckTypes();
            if (!Type.IsArith())
            {
                $"\"{Type.Value}\" cannot be used in arithmetic expression".RaiseError(Operator.Line);
                return null;
            }

            return Type;
        }

        public override void GenerateCode(CodeWriter w)
        {
            Expression.GenerateCode(w);
            switch (Expression.Type.Value)
            {
                case "int":
                    w.Write(Instr.I_INT_NEG);
                    break;
                case "float":
                    w.Write(Instr.I_FLOAT_NEG);
                    break;
                default:
                    throw  new Exception("shiet");
            }
        }

    }
}
