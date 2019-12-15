using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.CodeGeneration;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions;
using AbstractSyntaxTree_Implementation.ResolveNames;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements
{
    public class Assign : Statement
    {
        public TokenNode Variable { get; set; } // reikia targeto
        public Expression Expression { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(Variable), Variable);
            p.Print(nameof(Expression), Expression);
        }

        public override void ResolveNames(Scope scope)
        {
            Variable.ResolveNames(scope);
            Expression.ResolveNames(scope);
        }

        public override Type CheckTypes()
        {
            var type = (Type)Variable.Target?.GetType().GetProperty("Type")?.GetMethod.Invoke(Variable.Target, null);

            type?.IsCompatible(Expression.CheckTypes());

            return null;
        }

        public override void GenerateCode(CodeWriter w)
        {
            Expression.GenerateCode(w);

            switch (Variable.Target)
            {
                case Parameter p:
                    w.Write(Instr.I_SET_L, p.StackSlot);
                    break;
                case LocalVariableDeclaration l:
                    w.Write(Instr.I_SET_L, l.StackSlot);
                    break;
                case VariableDeclaration v:
                    w.Write(Instr.I_SET_H, v.HeapSlot);
                    break;
                //TODO add member access expression

            }
        }
    }
}
