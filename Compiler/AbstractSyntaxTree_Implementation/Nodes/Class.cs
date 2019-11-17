using Lexer_Implementation.DynamicLexer;

namespace AbstractSyntaxTree_Implementation.Nodes
{
    public class Class : Node
    {
        public LexemeNode Name { get; set; }
        public LexemeNode Extends { get; set; }
        public ClassBody Body { get; set; }
    }
}