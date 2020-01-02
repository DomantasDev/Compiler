using System;
using System.Collections.Generic;
using System.Linq;
using AbstractSyntaxTree_Implementation.ResolveNames;
using CodeGeneration.CodeGeneration;
using Common;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers
{
    public class Method : ClassMember
    {
        public static int LocalVariableCount { get; set; }
        public Visibility Visibility { get; set; }
        public TokenNode Virtual_Override { get; set; }
        public Type ReturnType { get; set; }
        public List<Parameter> Parameters { get; set; }
        public Body Body { get; set; }

        public int NumLocals { get; set; }
        public int VTableSlot { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(Visibility), Visibility);
            p.Print(nameof(Virtual_Override), Virtual_Override);
            p.Print(nameof(ReturnType), ReturnType);
            p.Print(nameof(Name), Name);
            p.Print(nameof(Parameters), Parameters);
            p.Print(nameof(Body), Body);
        }

        public override void ResolveNames(Scope scope)
        {
            LocalVariableCount = 0;

            scope = new Scope(scope);
            ReturnType.ResolveNames(scope);
            Parameters?.ForEach(x => x.ResolveNames(scope));
            Body?.ResolveNames(scope);

            NumLocals = LocalVariableCount;
        }

        public override void AddName(Scope scope)
        {
            scope.Add(new Name(Name, NameType.Method), this);
        }

        public override Type CheckTypes()
        {
            if (Virtual_Override != null)
            {
                var refType = FindAncestor<Class>().Extends;
                if (refType == null)
                {
                    $"Method {Name.Value}, cannot be marked with {Virtual_Override.Value} because this class doesn't extend any other class".RaiseError(Name.Line);
                }
                else
                {
                    while (refType != null)
                    {
                        var method = (Method)refType.TargetClass?.Body?.Members.FirstOrDefault(x => x is Method m && m.Name.Value == Name.Value);
                        if (method == null)
                        {
                            refType = refType.TargetClass?.Extends;
                        }
                        else
                        {
                            var expectedParamCount = method.Parameters?.Count ?? 0;
                            var actualParamCount = Parameters?.Count ?? 0;

                            if (expectedParamCount != actualParamCount)
                                $"Expected {expectedParamCount} parameters, got {actualParamCount}".RaiseError(Name.Line);

                            for (int i = 0; i < Math.Min(expectedParamCount, actualParamCount); i++)
                            {
                                method.Parameters?[i].Type.IsEqual(Parameters?[i].Type);
                            }

                            break;
                        }
                    }
                    if(refType == null)
                        $"Method {Name.Value} doesn't exist on base type".RaiseError(Name.Line);
                }
            }

            Body?.CheckTypes();

            return null;
        }

        public override void GenerateCode(CodeWriter w)
        {
            w.PlaceLabel(StartLabel);
            var localVariables = NumLocals - (Parameters?.Count ?? 0);
            if (localVariables > 0)
                w.Write(Instr.I_ALLOC_S, localVariables);
            Body.GenerateCode(w);
            w.Write(Instr.I_RET);
        }
    }
}
