using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractSyntaxTree_Implementation.Nodes
{
    public interface IDeclaration
    {
        Node Target { get; }
    }
}
