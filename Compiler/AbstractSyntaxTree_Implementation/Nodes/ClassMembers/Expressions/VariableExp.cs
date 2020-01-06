using System;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements;
using AbstractSyntaxTree_Implementation.ResolveNames;
using CodeGeneration.CodeGeneration;
using Common;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions
{
    public class VariableExp : Expression, ITokenNode
    {
        public Node Target { get; set; }
        public string TokenType { get; set; }
        public string Value { get; set; }
        public int Line { get; set; }

        public override void ResolveNames(Scope scope)
        {
            Target = scope.ResolveName(new Name(this, NameType.Variable), true);
        }

        public override Type CheckTypes()
        {
            if (Target == null)
            {
                var parentClass = FindAncestor<Class>();
                Target = Scope.ResolveForClass(parentClass.Name.Value, new Name(this, NameType.Variable));
            }
            Type = (Type)Target?.GetType().GetProperty("Type")?.GetMethod.Invoke(Target, null);
            return Type;
        }

        public override void GenerateCode(CodeWriter w)
        {
            switch (Target)
            {
                case LocalVariableDeclaration l:
                    w.Write(Instr.I_GET_L, l.StackSlot);
                    break;
                case Parameter p:
                    w.Write(Instr.I_GET_L, p.StackSlot);
                    break;
                case VariableDeclaration h:
                    w.Write(Instr.I_GET_C);
                    w.Write(Instr.I_GET_H, h.HeapSlot);
                    break;
                default:
                    throw new Exception($"{nameof(VariableExp)}".UnexpectedError(Line));
            }
        }
    }
}
