using System;
using System.Collections.Generic;
using System.Text;
using Lexer_Implementation.DynamicLexer;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions
{
    public class UnaryExp : Expression
    {
        public LexemeNode Operator { get; set; }
        public Expression Right { get; set; }
    }
}
