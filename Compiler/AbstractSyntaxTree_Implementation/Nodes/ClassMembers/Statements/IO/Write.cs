using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions;
using AbstractSyntaxTree_Implementation.Nodes.Types;
using AbstractSyntaxTree_Implementation.ResolveNames;
using CodeGeneration.CodeGeneration;
using Common;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements.IO
{
    public class Write : Statement
    {
        public List<Expression> Arguments { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(Arguments), Arguments);
        }

        public override void ResolveNames(Scope scope)
        {
            Arguments.ForEach(x => x.ResolveNames(scope));
        }

        public override Type CheckTypes()
        {
            foreach (var arg in Arguments)
            {
                var argType = arg.CheckTypes();
                if (argType is ReferenceType)
                    $"Only primitive types are allowed in write statement. Got {argType.Value}"
                        .RaiseError(argType.Line);
            }

            return null;
        }

        public override void GenerateCode(CodeWriter w)
        {
            
            for (var i = Arguments.Count - 1; i >= 0; i--)
            {
                Arguments[i].GenerateCode(w);
                var primitiveType = Arguments[i].Type.GetPrimitiveType();
                w.Write(Instr.I_PUSH ,(int)primitiveType);
            }

            w.Write(Instr.I_WRITE, Arguments.Count);
        }
    }
}
