using System;
using System.Linq;
using AbstractSyntaxTree_Implementation.ResolveNames;
using AbstractSyntaxTree_Implementation.Nodes.Types;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes
{
    public abstract class Node
    {
        public Node Parent { get; set; }

        public T FindAncestor<T>() where T : Node
        {
            var node = Parent;
            while (node != null)
            {
                if (node.GetType() == typeof(T))
                    return (T)node;
                node = node.Parent;
            }

            return null;
        }
        public void AddChildren(params Node[] children)
        {
            foreach (var child in children ?? Enumerable.Empty<Node>())
            {
                if(child != null)
                    child.Parent = this;
            }
        }
        public virtual void Print(NodePrinter p)
        {
            throw new NotImplementedException($"{nameof(Print)} not implemented for {GetType()}");
        }

        public virtual void ResolveNames(Scope scope)
        {
            throw new NotImplementedException($"{nameof(ResolveNames)} not implemented for {GetType()}");
        }

        public virtual Type CheckTypes()
        {
            throw new NotImplementedException($"{nameof(ResolveNames)} not implemented for {GetType()}");
        }
    }
}
