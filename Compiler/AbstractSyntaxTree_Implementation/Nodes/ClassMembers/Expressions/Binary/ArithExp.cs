using System;
using Common;
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

            Type = leftType;

            if (leftType.IsArith())
            {
                leftType.IsEqual(rightType);
            }
            else
            {
                $"Values: \"{leftType.Value}\", \"{rightType.Value}\", cannot be used in arithmetic expression".RaiseError(Operator.Line);
            }

            return new ValueType
            {
                Value = leftType.Value,
                Line = Operator.Line
            };
        }
    }
}
