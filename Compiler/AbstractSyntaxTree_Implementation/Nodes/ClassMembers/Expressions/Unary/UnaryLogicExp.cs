using System;
using System.Collections.Generic;
using System.Text;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions.Unary
{
    public class UnaryLogicExp : UnaryExp
    {
        public override Type CheckTypes()
        {
            var type = Expression.CheckTypes();
            if (type.Value != "bool")
            {
                Console.WriteLine($"{type.Value} cannot be used in logic expression");
                return null;
            }

            return type;
        }
    }
}
