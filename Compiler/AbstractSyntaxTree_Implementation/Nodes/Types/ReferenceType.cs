﻿using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.ResolveNames;

namespace AbstractSyntaxTree_Implementation.Nodes.Types
{
    public class ReferenceType : Type
    {
        public new Class Target { get; set; }
        public override void ResolveNames(Scope scope)
        {
            Target = (Class)scope.ResolveName(new Name(this, NameType.Class));
        }

        public override void IsCompatible(Type other)
        {
            if (!(other is ReferenceType otherRefType))
                return;

            do
            {
                if (Value == otherRefType.Value)
                    return;

                otherRefType = otherRefType.Target.Extends;
            } while (otherRefType != null);
            
        }
    }
}