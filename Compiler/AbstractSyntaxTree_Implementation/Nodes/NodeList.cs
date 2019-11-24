using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractSyntaxTree_Implementation.Nodes
{
    public class NodeList : Node
    {
        public List<Node> Nodes { get; set; }
    }
}
