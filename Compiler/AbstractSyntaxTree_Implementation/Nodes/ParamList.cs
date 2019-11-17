using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractSyntaxTree_Implementation.Nodes
{
    public class ParamList : Node
    {
        public List<Parameter> Parameters { get; set; }
    }
}
