using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.ResolveNames;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions
{
    public class CallExp : Expression
    {
        public Method Target { get; set; }
        public TokenNode MethodName { get; set; }
        public List<Expression> Arguments { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(MethodName), MethodName);
            p.Print(nameof(Arguments), Arguments);
        }

        public override void ResolveNames(Scope scope)
        {
            Target = (Method)scope.ResolveName(new Name(MethodName, NameType.Method));
            Arguments?.ForEach(x => x.ResolveNames(scope));
            //handle method name
        }

        public override Type CheckTypes()
        {
            if (Target != null)
            {
                var expectedParamCount = Target.Parameters.Count;
                var actualParamCount = Arguments.Count;

                if (expectedParamCount != actualParamCount)
                    Console.WriteLine($"Expected {actualParamCount} parameters, got {actualParamCount}");

                for (int i = 0; i < Math.Min(expectedParamCount, actualParamCount); i++)
                {
                    Target.Parameters[i].Type.IsEqual(Arguments[i].CheckTypes());
                }
            }

            return Target?.ReturnType;
        }
    }
}
