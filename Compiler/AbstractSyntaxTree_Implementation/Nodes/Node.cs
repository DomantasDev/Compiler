using System;

namespace AbstractSyntaxTree_Implementation.Nodes
{
    public abstract class Node
    {
        public virtual void Print(NodePrinter p)
        {
            throw new NotImplementedException($"print not implemented for {GetType()}");
        }
    }
}
