using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.CodeGeneration;
using AbstractSyntaxTree_Implementation.ResolveNames;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements.If
{
    public class Else : Node
    {
        public If If { get; set; }
        public Body Body { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(If), If);
            p.Print(nameof(Body), Body);
        }

        public override void ResolveNames(Scope scope)
        {
            If?.ResolveNames(scope);
            Body?.ResolveNames(new Scope(scope));
        }

        public override Type CheckTypes()
        {
            If?.CheckTypes();
            Body?.CheckTypes();

            return null;
        }

        public override void GenerateCode(CodeWriter w)
        {
            If?.GenerateCode(w);
            Body?.GenerateCode(w);
        }
    }
}
