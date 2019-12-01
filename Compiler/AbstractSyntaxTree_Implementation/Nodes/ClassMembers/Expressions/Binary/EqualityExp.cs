using System;
using System.Collections.Generic;
using System.Text;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;
using ValueType = AbstractSyntaxTree_Implementation.Nodes.Types.ValueType;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions.Binary
{
    public class EqualityExp : BinaryExp
    {
        public override Type CheckTypes()
        {
            var leftType = Left.CheckTypes();
            var rightType = Right.CheckTypes();

            if (leftType.IsEquatable())
            {
                leftType.IsEqual(rightType);
            }
            else
            {
                Console.WriteLine($"These values cannot be checked for equality: {leftType.Value}, {rightType.Value}. Line {Operator.Line}");
            }

            return new ValueType
            {
                Value = "bool",
                Line = Operator.Line
            };
        }
    }
}

