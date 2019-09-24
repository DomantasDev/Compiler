namespace Lexer_Implementation
{
    public enum LexemType
    {
        Ident,

        KW_if,
        KW_else,
        KW_while,
        KW_int,
        KW_bool,
        KW_string,
        KW_return,
        KW_class,
        KW_program,
        KW_break,
        KW_continue,

        Op_dot,
        Op_plus,
        Op_minus,
        Op_mult,
        Op_div,
        Op_greater,
        Op_greater_e,
        Op_less,
        Op_less_e,
        Op_equal,
        Op_not_equal,
        Op_or,
        Op_and,
        Op_not,
        Op_brace_o,
        Op_brace_c,
        Op_paren_o,
        Op_paren_c,

        Lit_string,
        Lit_int,
        Lit_float,
        Lit_bool,

        EOF
    }
}
