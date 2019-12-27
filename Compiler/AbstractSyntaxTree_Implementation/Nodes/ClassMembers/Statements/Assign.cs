using System;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions.Binary.Dot;
using AbstractSyntaxTree_Implementation.ResolveNames;
using CodeGeneration.CodeGeneration;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements
{
    public class Assign : Statement
    {
        public Node Variable { get; set; } // reikia targeto
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
            Type type;
            if (Variable is TokenNode tokenNode)
            {
                type = (Type)tokenNode.Target?.GetType().GetProperty("Type")?.GetMethod.Invoke(tokenNode.Target, null);
            }
            else if (Variable is MemberAccessExp memberAccess)
            {
                type = memberAccess.CheckTypes();
            }
            else
            {
                throw new Exception("bad stuff");
            }

            type?.IsCompatible(Expression.CheckTypes());

            return null;
        }

        public override void GenerateCode(CodeWriter w)
        {
            Expression.GenerateCode(w);
            if (Variable is MemberAccessExp memberAccess)
            {
                memberAccess.Left.GenerateCode(w);
                var fieldHeapSlot = ((VariableDeclaration)((VariableExp)memberAccess.Right).Target).HeapSlot;
                w.Write(Instr.I_PUSH, fieldHeapSlot);
                w.Write(Instr.I_INT_ADD);
                w.Write(Instr.I_SET_A);
            }
            else if (Variable is TokenNode variable)
            {
                switch (variable.Target)
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
}
