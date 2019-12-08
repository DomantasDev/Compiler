using System;
using AbstractSyntaxTree_Implementation.ResolveNames;
using Common;

namespace AbstractSyntaxTree_Implementation.Nodes.Types
{
    public abstract class Type : TokenNode
    {
        public override void ResolveNames(Scope scope)
        {
            //do nothing
        }

        public abstract void IsCompatible(Type other);

        public void IsEqual(Type other)
        {
            if(other == null)
                return;
            if (GetType() != other.GetType() || Value != other.Value)
                TypeMismatch(this, other);
        }

        protected void TypeMismatch(Type expected, Type got)
        {
            $"Type mismatch. Expected {expected.Value} got {got.Value}".RaiseError(expected.Line);
        }

        public bool IsArith()
        {
            return Value == "int" || Value == "float";
        }

        public bool IsComparable()
        {
            return IsArith();
        }

        public bool IsEquatable()
        {
            return this is ValueType;
        }
    }
}
