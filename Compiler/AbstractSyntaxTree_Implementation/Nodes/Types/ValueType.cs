﻿using System;
using System.Collections.Generic;
using System.Text;
using CodeGeneration.CodeGeneration;

namespace AbstractSyntaxTree_Implementation.Nodes.Types
{
    public class ValueType : Type
    {
        public override void IsCompatible(Type other)
        {
            if (other == null)
                return;

            if (other.GetType() != typeof(ValueType) || Value != other.Value)
            {
                TypeMismatch(this, other);
            }
        }

        public override void IsEquatable(Type other)
        {
            if (!(other is ValueType otherValueType) || otherValueType.Value != Value)
            {
                EqualityTypeMismatch(this, other);
                return;
            }

            if (otherValueType.Value == "null")
            {
                TypeMismatch(this, other);
                return;
            }
        }

        public override PrimitiveType GetPrimitiveType()
        {
            switch (Value)
            {
                case "int":
                    return PrimitiveType.Int;
                case "float":
                    return PrimitiveType.Float;
                case "bool":
                    return PrimitiveType.Bool;
                default:
                    throw new Exception("bad stuff");
            }
        }
    }
}
