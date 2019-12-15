using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes.Types;
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
         
    }
}
