using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using Lexer_Implementation.DynamicLexer;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions
{
    public class BinaryExp : Expression
    {
        public Expression Left { get; set; }
        public LexemeNode Operator { get; set; }
        public Expression Right { get; set; }
    }
}
