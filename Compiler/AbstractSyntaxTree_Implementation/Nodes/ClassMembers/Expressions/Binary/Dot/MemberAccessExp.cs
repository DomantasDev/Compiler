using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes.Types;
using AbstractSyntaxTree_Implementation.ResolveNames;
using CodeGeneration.CodeGeneration;
using Common;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions.Binary.Dot
{
    public class MemberAccessExp : MemberExp
    {
        public override Type CheckTypes()
        {
            var varExp = (VariableExp)Right;
            Type = varExp.CheckTypes();
            var leftType = Left.CheckTypes();

            if (!(leftType is ReferenceType refType))
            {
                $"Only a reference type can be on the left side of member access expression.".RaiseError(Operator.Line);
            }
            else
            {

                while (refType != null)
                {
                    var variable = (VariableDeclaration)refType.TargetClass?.Body.Members.FirstOrDefault(x =>
                        x is VariableDeclaration v &&
                        v.Name.Value == varExp.Value);

                    if (variable == null)
                    {
                        refType = refType.TargetClass?.Extends;
                    }
                    else
                    {
                        varExp.Target = variable;
                        return variable.Type;
                    }
                }

                $"{leftType.Value} doesn't contain a member named {varExp.Value}.".RaiseError(varExp.Line);
            }

            return null;
        }

        public override void GenerateCode(CodeWriter w)
        {
            Left.GenerateCode(w);
            var fieldHeapSlot = ((VariableDeclaration)((VariableExp)Right).Target).HeapSlot;
            w.Write(Instr.I_GET_H, fieldHeapSlot);
        }
    }
}
