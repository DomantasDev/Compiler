using System;
using System.Collections.Generic;
using System.Text;

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
                return;
            }
        }
    }
}
