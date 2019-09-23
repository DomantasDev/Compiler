namespace Lexer_Implementation
{
    public class Token
    {
        public string Value { get; set; }
        public int Line { get; set; }
        public LexemType Type { get; set; }
    }
}
