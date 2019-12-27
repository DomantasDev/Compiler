using System;
using System.Linq;
using AbstractSyntaxTree_Implementation.Nodes.Types;
using AbstractSyntaxTree_Implementation.ResolveNames;
using CodeGeneration.CodeGeneration;
using Common;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions.Binary.Dot
{
    public class MemberCallExp : MemberExp
    {
        public override void ResolveNames(Scope scope)
        {
            Left.ResolveNames(scope);
        }

        public override Type CheckTypes()
        {
            var callExp = (CallExp)Right;
            var leftType = Left.CheckTypes();

            if (!(leftType is ReferenceType refType))
            {
                $"Only a reference type can be on the left side of member access expression. Line{Operator.Line}"
                    .RaiseError(callExp.MethodName.Line);
            }
            else
            {

                while (refType != null)
                {
                    var method = (Method)refType.TargetClass?.Body.Members.FirstOrDefault(x =>
                            x is Method v &&
                            v.Name.Value == callExp.MethodName.Value);

                    if (method == null)
                    {
                        refType = refType.TargetClass?.Extends;
                    }
                    else
                    {
                        var expectedParamCount = method.Parameters?.Count ?? 0;
                        var actualParamCount = callExp.Arguments?.Count ?? 0;

                        if (expectedParamCount != actualParamCount)
                            $"Expected {expectedParamCount} parameters, got {actualParamCount}"
                                .RaiseError(callExp.MethodName.Line);

                        for (int i = 0; i < Math.Min(expectedParamCount, actualParamCount); i++)
                        {
                            method.Parameters?[i].Type.IsCompatible(callExp.Arguments?[i].CheckTypes());
                        }

                        callExp.TargetMethod = method;
                        Type = callExp.TargetMethod.ReturnType;
                        return method.ReturnType;
                    }
                }

                $"{leftType.Value} doesn't contain a method named {callExp.MethodName.Value}"
                    .RaiseError(callExp.MethodName.Line);
            }

            return null;
        }

        public override void GenerateCode(CodeWriter w)
        {
            var callExp = (CallExp)Right;
            callExp.TargetExpression = Left;
            callExp.GenerateCode(w);
        }
    }
}
