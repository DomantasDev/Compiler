using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.ResolveNames;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes
{
    public class Program : Node
    {
        public List<Class> CLasses { get; set; }
        public override void Print(NodePrinter p)
        {
            p.Print("Classes", CLasses);
        }

        public override void ResolveNames(Scope scope)
        {
            var childrenScopes = new List<Scope>();

            for (var i = 0; i < CLasses.Count; i++)
            {
                var clas = CLasses[i];

                childrenScopes.Add(new Scope(scope, clas.Name.Value));

                scope.Add(new Name(clas.Name, NameType.Class), clas);
                Scope.ClassTable.Add(clas.Name.Value, clas);
                clas.AddNames(childrenScopes[i]);
            }

            for (var i = 0; i < CLasses.Count; i++)
            {
                CLasses[i].ResolveNames(childrenScopes[i]);
            }
        }

        public override Type CheckTypes()
        {
            CLasses.ForEach(x => x.CheckTypes());

            return null;
        }
    }
}
