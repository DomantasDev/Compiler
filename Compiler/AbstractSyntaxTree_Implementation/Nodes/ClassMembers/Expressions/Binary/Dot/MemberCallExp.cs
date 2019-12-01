using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes.Types;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions.Binary.Dot
{
    public class MemberCallExp : MemberExp
    {
        public override Type CheckTypes()
        {
            var callExp = (CallExp)Right;
            var leftType = Left.CheckTypes();

            if (!(leftType is ReferenceType refType))
            {
                Console.WriteLine($"Only a reference type can be on the left side of member access expression. Line{Operator.Line}");
            }
            else
            {

                while (refType != null)
                {
                    var method = (Method)refType.Target?.Body.Members.FirstOrDefault(x =>
                        x is Method v &&
                        v.Name.Value == callExp.MethodName.Value);

                    if (method == null)
                    {
                        refType = refType.Target?.Extends;
                    }
                    else
                    {
                        var expectedParamCount = method.Parameters?.Count ?? 0;
                        var actualParamCount = callExp.Arguments?.Count ?? 0;

                        if (expectedParamCount != actualParamCount)
                            Console.WriteLine($"Expected {expectedParamCount} parameters, got {actualParamCount}");

                        for (int i = 0; i < Math.Min(expectedParamCount, actualParamCount); i++)
                        {
                            method.Parameters?[i].Type.IsCompatible(callExp.Arguments?[i].CheckTypes());
                        }

                        return method.ReturnType;
                    }
                }
                Console.WriteLine($"{leftType.Value} doesn't contain a method named {callExp.MethodName.Value}");
            }

            return null;
        }
    }
}
