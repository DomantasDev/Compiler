using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes.Types;
using AbstractSyntaxTree_Implementation.ResolveNames;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions.Binary.Dot
{
    public class MemberAccessExp : MemberExp
    {
        public override Type CheckTypes()
        {
            var varExp = (VariableExp)Right;
            var leftType = Left.CheckTypes();

            if (!(leftType is ReferenceType refType))
            {
                Console.WriteLine($"Only a reference type can be on the left side of member access expression. Line{Operator.Line}");
            }
            else
            {

                while (refType != null)
                {
                    var variable = (VariableDeclaration)refType.Target?.Body.Members.FirstOrDefault(x =>
                        x is VariableDeclaration v &&
                        v.Name.Value == varExp.Value);

                    if (variable == null)
                    {
                        refType = refType.Target?.Extends;
                    }
                    else
                    {
                        return variable.Type;
                    }
                }
                Console.WriteLine($"{leftType.Value} doesn't contain a member named {varExp.Value}");
            }

            return null;
        }
    }
}
