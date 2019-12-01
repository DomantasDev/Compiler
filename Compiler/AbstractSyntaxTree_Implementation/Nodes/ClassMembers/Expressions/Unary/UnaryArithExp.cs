using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes.Types;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;
using ValueType = AbstractSyntaxTree_Implementation.Nodes.Types.ValueType;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions.Unary
{
    public class UnaryArithExp : UnaryExp
    {
        public override Type CheckTypes()
        {
            var type = Expression.CheckTypes();
            if (!type.IsArith())
            {
                Console.WriteLine($"{type.Value} cannot be used in arithmetic expression");
                return null;
            }

            return type;
        }
         
    }
}
