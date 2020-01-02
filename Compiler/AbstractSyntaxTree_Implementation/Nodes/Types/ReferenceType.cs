using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.ResolveNames;
using CodeGeneration.CodeGeneration;

namespace AbstractSyntaxTree_Implementation.Nodes.Types
{
    public class ReferenceType : Type
    {
        public Class TargetClass => (Class) Target;
        public override void ResolveNames(Scope scope)
        {
            Target = scope.ResolveName(new Name(this, NameType.Class));
        }

        public override void IsCompatible(Type other)
        {
            if (!(other is ReferenceType otherRefType))
            {
                TypeMismatch(this, other);
                return;
            }

            do
            {
                if (Value == otherRefType.Value)
                    return;

                otherRefType = ((Class)otherRefType.Target).Extends;
            } while (otherRefType != null);
            

        }

        public override PrimitiveType GetPrimitiveType()
        {
            throw new Exception("bad stuff");
        }
    }
}
