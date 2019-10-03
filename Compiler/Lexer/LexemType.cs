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
        KW_float,
        KW_void,
        KW_null,
        KW_return,
        KW_class,
        KW_break,
        KW_continue,
        KW_virtual,
        KW_override,
        KW_private,
        KW_protected,
        KW_public,
        KW_this,
        KW_base,
        KW_constructor,
        KW_new,
        KW_extends,
        KW_write,
        KW_read,

        Op_or,
        Op_and,
        Op_plus,
        Op_minus,
        Op_mult,
        Op_div,
        Op_dot,
        Op_not,
        Op_greater,
        Op_greater_e,
        Op_less,
        Op_less_e,
        Op_equal,
        Op_not_equal,
        Op_assign,
        Op_comma,
        Op_col,
        OP_semicol,

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
