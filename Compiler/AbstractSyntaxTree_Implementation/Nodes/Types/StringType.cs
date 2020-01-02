using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.ResolveNames;
using CodeGeneration.CodeGeneration;

namespace AbstractSyntaxTree_Implementation.Nodes.Types
{
    public class StringType : Type
    {
        public override void IsCompatible(Type other)
        {
            if (!(other is StringType))
                TypeMismatch(this, other);
        }

        public override PrimitiveType GetPrimitiveType()
        {
            return PrimitiveType.String;
        }
    }
}
