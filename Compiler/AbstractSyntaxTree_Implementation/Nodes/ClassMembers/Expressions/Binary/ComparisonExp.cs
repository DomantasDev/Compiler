﻿using System;
using System.Collections.Generic;
using System.Text;
using Common;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;
using ValueType = AbstractSyntaxTree_Implementation.Nodes.Types.ValueType;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions.Binary
{
    public class ComparisonExp : BinaryExp
    {
        public override Type CheckTypes()
        {
            var leftType = Left.CheckTypes();
            var rightType = Right.CheckTypes();

            Type = new ValueType
            {
                Value = "bool"
            };

            if (leftType.IsComparable())
            {
                leftType.IsEqual(rightType);
            }
            else
            {
                $"Values: \"{leftType.Value}\", \"{rightType.Value}\", cannot be compared".RaiseError(Operator.Line);
            }

            return new ValueType
            {
                Value = "bool",
                Line = Operator.Line
            };
        }
    }
}
