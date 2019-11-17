using System;

namespace AbstractSyntaxTree_Implementation.Nodes
{
    public abstract class Node
    {
        public virtual void Print()
        {
            throw new NotImplementedException($"print not implemented for {GetType()}");
        }
    }
}
