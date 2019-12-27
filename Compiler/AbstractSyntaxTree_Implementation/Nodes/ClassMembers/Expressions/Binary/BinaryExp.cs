using AbstractSyntaxTree_Implementation.ResolveNames;
using CodeGeneration.CodeGeneration;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions.Binary
{
    public abstract class BinaryExp : Expression
    {
        public virtual Expression Left { get; set; }
        public virtual TokenNode Operator { get; set; }
        public virtual Expression Right { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(Left), Left);
            p.Print(nameof(Operator), Operator);
            p.Print(nameof(Right), Right);
        }

        public override void ResolveNames(Scope scope)
        {
            Left.ResolveNames(scope);
            Right.ResolveNames(scope);
        }

        public override void GenerateCode(CodeWriter w)
        {
            Left.GenerateCode(w);
            Right.GenerateCode(w);

            GenerateOperator(w);
        }

        private void GenerateOperator(CodeWriter w)
        {
            if (Operator.TokenType == "EQUALS_OP")
            {
                switch (Operator.Value)
                {
                    case "=":
                        w.Write(Instr.I_EQ);
                        break;
                    case "<>":
                        w.Write(Instr.I_NEQ);
                        break;
                }
            }
            else
            {
                switch (Left.Type.Value)
                {
                    case "int":
                        Int(w);
                        break;
                    case "float":
                        Float(w);
                        break;
                    case "bool":
                        Bool(w);
                        break;
                }
            }
        }

        private void Int(CodeWriter w)
        {
            switch (Operator.Value)
            {
                case "+":
                    w.Write(Instr.I_INT_ADD);
                    break;
                case "-":
                    w.Write(Instr.I_INT_SUB);
                    break;
                case "*":
                    w.Write(Instr.I_INT_MUL);
                    break;
                case "/":
                    w.Write(Instr.I_INT_DIV);
                    break;
                case "<":
                    w.Write(Instr.I_INT_LESS);
                    break;
                case "<=":
                    w.Write(Instr.I_INT_LESSEQ);
                    break;
                case ">":
                    w.Write(Instr.I_INT_GR);
                    break;
                case ">=":
                    w.Write(Instr.I_INT_GREQ);
                    break;
            }
        }

        private void Float(CodeWriter w)
        {
            switch (Operator.Value)
            {
                case "+":
                    w.Write(Instr.I_FLOAT_ADD);
                    break;          
                case "-":           
                    w.Write(Instr.I_FLOAT_SUB);
                    break;          
                case "*":           
                    w.Write(Instr.I_FLOAT_MUL);
                    break;          
                case "/":           
                    w.Write(Instr.I_FLOAT_DIV);
                    break;
                case "<":
                    w.Write(Instr.I_FLOAT_LESS);
                    break;
                case "<=":
                    w.Write(Instr.I_FLOAT_LESSEQ);
                    break;
                case ">":
                    w.Write(Instr.I_FLOAT_GR);
                    break;
                case ">=":
                    w.Write(Instr.I_FLOAT_GREQ);
                    break;
            }
        }

        private void Bool(CodeWriter w)
        {
            switch (Operator.Value)
            {
                case "&":
                    w.Write(Instr.I_AND);
                    break;
                case "|":
                    w.Write(Instr.I_OR);
                    break;
            }
        }
    }
}
