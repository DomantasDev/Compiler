using Lexer_Implementation.DynamicLexer;

namespace AbstractSyntaxTree_Implementation.Nodes
{
    public class Parameter : Node
    {
        public LexemeNode Name { get; set; }
        public LexemeNode Type { get; set; }
    }
}