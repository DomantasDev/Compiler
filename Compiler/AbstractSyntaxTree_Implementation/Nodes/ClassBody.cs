using System.Collections.Generic;
using AbstractSyntaxTree_Implementation.CodeGeneration;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers;
using AbstractSyntaxTree_Implementation.Nodes.Types;
using AbstractSyntaxTree_Implementation.ResolveNames;

namespace AbstractSyntaxTree_Implementation.Nodes
{
    public class ClassBody : Node
    {
        public List<ClassMember> Members { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print("Members", Members);
        }

        public override void ResolveNames(Scope scope)
        {
            Members.ForEach(x => x.ResolveNames(scope));
        }

        public void AddNames(Scope scope)
        {
            Members.ForEach(x => x.AddName(scope));
        }

        public override Type CheckTypes()
        {
            Members.ForEach(x => x.CheckTypes());

            return null;
        }

        public override void GenerateCode(CodeWriter w)
        {
            Members.ForEach(x => x.GenerateCode(w));
        }
    }
}