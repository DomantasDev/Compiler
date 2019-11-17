using System.Collections.Generic;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers
{
    public class MethodBody : Node
    {
        public List<Statement> Statements { get; set; }
    }
}