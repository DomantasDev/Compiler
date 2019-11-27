using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements
{
    public class LocalVariableDeclaration : Statement
    {
        public Type Type { get; set; }
        public TokenNode Name { get; set; }
        public Expression Expression { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(Type), Type);
            p.Print(nameof(Name), Name);
            p.Print(nameof(Expression), Expression);
        }
    }
}
