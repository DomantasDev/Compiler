namespace Lexer_Implementation
{
    public enum LexemType
    {
        Ident,

        KW_if,
        KW_while,
        KW_int,
        KW_bool,
        KW_string,

        Op_dot,

        Lit_string,
        Lit_int,
        Lit_float,
        Lit_bool,
        EOF
    }
}
