using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements.IO
{
    public class Read : Statement
    {
        public List<TokenNode> Variables { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(Variables), Variables);
        }
    }
}
