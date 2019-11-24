using System;
using System.Collections.Generic;
using System.Text;
using Lexer_Implementation.DynamicLexer;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions
{
    public abstract class UnaryExp : Expression
    {
        public TokenNode Operator { get; set; }
        public Expression Expression { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(Operator), Operator);
            p.Print(nameof(Expression), Expression);
        }
    }
}
