using System;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;
using ValueType = AbstractSyntaxTree_Implementation.Nodes.Types.ValueType;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions.Binary
{
    public class ArithExp : BinaryExp
    {
        public override Type CheckTypes()
        {
            var leftType = Left.CheckTypes();
            var rightType = Right.CheckTypes();

            if (Left.CheckTypes().IsArith())
            {
                leftType.IsEqual(rightType);
            }
            else
            {
                Console.WriteLine($"These values cannot be used in arithmetic expression: {leftType.Value}, {rightType.Value}. Line {Operator.Line}");
            }

            return new ValueType
            {
                Value = "int",
                Line = Operator.Line
            };
        }
    }
}
