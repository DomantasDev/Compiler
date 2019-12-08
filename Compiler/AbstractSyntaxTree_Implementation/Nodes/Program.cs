using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbstractSyntaxTree_Implementation.CodeGeneration;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers;
using AbstractSyntaxTree_Implementation.ResolveNames;
using Common;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes
{
    public class Program : Node
    {
        public List<Class> Classes { get; set; }
        public override void Print(NodePrinter p)
        {
            p.Print("Classes", Classes);
        }

        public override void ResolveNames(Scope scope)
        {
            FindEntryPoint();

            var childrenScopes = new List<Scope>();

            for (var i = 0; i < Classes.Count; i++)
            {
                var clas = Classes[i];

                childrenScopes.Add(new Scope(scope, clas.Name.Value));

                scope.Add(new Name(clas.Name, NameType.Class), clas);
                Scope.ClassTable.Add(clas.Name.Value, clas);
                clas.AddNames(childrenScopes[i]);
            }

            for (var i = 0; i < Classes.Count; i++)
            {
                Classes[i].ResolveNames(childrenScopes[i]);
            }
        }

        private void FindEntryPoint()
        {
            var entryClass = Classes.FirstOrDefault(x => x.Name.Value == "Program");

            var entryMethod = entryClass?.Body.Members.FirstOrDefault(x => x is Method m && m.Name.Value == "Main");

            if(entryClass == null)
                $"Class \"Program\" not found".RaiseError();

            if(entryMethod == null)
                $"Method \"Main\" not found in class \"Program\"".RaiseError();
        }

        public override Type CheckTypes()
        {
            Classes.ForEach(x => x.CheckTypes());

            return null;
        }

        public override void GenerateCode(CodeWriter w)
        {
            //TODO generate entry point

            Classes.ForEach(x => x.GenerateCode(w));
        }
    }
}
