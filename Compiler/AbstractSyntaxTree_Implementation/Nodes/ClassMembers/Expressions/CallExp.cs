using System;
using System.Collections.Generic;
using AbstractSyntaxTree_Implementation.ResolveNames;
using CodeGeneration.CodeGeneration;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions
{
    public class CallExp : Expression
    {
        public Method TargetMethod { get; set; }
        public TokenNode MethodName { get; set; }
        public List<Expression> Arguments { get; set; }

        public Expression TargetExpression { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(MethodName), MethodName);
            p.Print(nameof(Arguments), Arguments);
        }

        public override void ResolveNames(Scope scope)
        {
            TargetMethod = (Method)scope.ResolveName(new Name(MethodName, NameType.Method));
            Arguments?.ForEach(x => x.ResolveNames(scope));
            //handle method name
        }

        public override Type CheckTypes()
        {
            if (TargetMethod != null)
            {
                Type = TargetMethod.ReturnType;
                var expectedParamCount = TargetMethod.Parameters?.Count ?? 0;
                var actualParamCount = Arguments?.Count ?? 0;

                if (expectedParamCount != actualParamCount)
                    Console.WriteLine($"Expected {actualParamCount} parameters, got {actualParamCount}");

                for (int i = 0; i < Math.Min(expectedParamCount, actualParamCount); i++)
                {
                    TargetMethod.Parameters?[i].Type.IsEqual(Arguments?[i].CheckTypes());
                }
            }

            return Type;
        }

        public override void GenerateCode(CodeWriter w)
        {
            w.Write(Instr.I_CALL_BEGIN);
            Arguments?.ForEach(x => x.GenerateCode(w));
            if (TargetExpression == null)
            {
                w.Write(Instr.I_GET_C);
            }
            else
            {
                TargetExpression.GenerateCode(w);
            }
            w.Write(Instr.I_VCALL, TargetMethod.VTableSlot, Arguments?.Count ?? 0);
        }
    }
}
