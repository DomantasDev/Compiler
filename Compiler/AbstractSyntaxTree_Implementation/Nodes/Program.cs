using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractSyntaxTree_Implementation.Nodes
{
    public class Program : Node
    {
        public List<Class> CLasses { get; set; }
        public override void Print(NodePrinter p)
        {
            p.Print("Classes", CLasses);
        }
    }
}
