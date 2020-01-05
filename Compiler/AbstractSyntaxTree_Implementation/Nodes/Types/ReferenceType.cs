using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.ResolveNames;
using CodeGeneration.CodeGeneration;
using Common;

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

            if(otherRefType.Value == "null")
                return;

            do
            {
                if (Value == otherRefType.Value)
                    return;

                otherRefType = ((Class)otherRefType.Target).Extends;
            } while (otherRefType != null);

            TypeMismatch(this, other);
        }

        public override void IsEquatable(Type other)
        {
            if (!(other is ReferenceType otherRefType))
            {
                EqualityTypeMismatch(this, other);
                return;
            }

            if (otherRefType.Value == "null")
                return;

            ReferenceType temp = otherRefType;
            do
            {
                if (Value == temp.Value)
                    return;

                temp = temp.TargetClass.Extends;
            } while (temp != null);

            temp = this;
            do
            {
                if (temp.Value == otherRefType.Value)
                    return;

                temp = temp.TargetClass.Extends;
            } while (temp != null);

            EqualityTypeMismatch(this, other);
        }

        public override PrimitiveType GetPrimitiveType()
        {
            throw new Exception("bad stuff");
        }
    }
}
