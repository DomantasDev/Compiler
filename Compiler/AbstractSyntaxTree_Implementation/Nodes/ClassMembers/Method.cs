using System;
using System.Collections.Generic;
using System.Text;
using Lexer_Implementation.DynamicLexer;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers
{
    public class Method : ClassMember
    {
        public LexemeNode Visibility { get; set; }
        public LexemeNode Virtual_Override { get; set; }
        public LexemeNode ReturnType { get; set; }
        public LexemeNode Name { get; set; }
        public ParamList Params { get; set; }
        public MethodBody Body { get; set; }
    }
}
