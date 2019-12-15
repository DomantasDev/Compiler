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

        private Class entryClass;
        private Method entryMethod;
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
            entryClass = Classes.FirstOrDefault(x => x.Name.Value == "Program");

            entryMethod = entryClass?.Body.Members.FirstOrDefault(x => x is Method m && m.Name.Value == "Main") as Method;

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
            GetHeapSlots();
            //TODO generate entry point. alloc(size), set Vtable address

            Classes.ForEach(x => x.GenerateCode(w));


            //TODO generate VTables
        }

        private void GetHeapSlots() // surasti metodu ir fieldu numerius
        {
            var classesByInheritanceLevel = Classes.Select(c => new
                {
                    Class = c,
                    Level = GetInheritanceLevel(c)
                }).GroupBy(x => x.Level,
                    (level,classes) => new
                    {
                        Level = level,
                        Classes = classes.Select(x => x.Class)
                    })
                .OrderBy(x => x.Level);

            foreach (var classDecl in classesByInheritanceLevel.SelectMany(x => x.Classes))
            {
                var parentVTableSlots = classDecl.Extends?.Target.VTableSlots;
                var vTableSlots = new Slots<Method>(parentVTableSlots);
                foreach (var method in classDecl.Body?.Members.OfType<Method>() ?? new List<Method>())
                {
                    vTableSlots.Add(method);
                    method.VTableSlot = vTableSlots.GetSlot(method).Value;
                }

                classDecl.VTableSlots = vTableSlots;

                var parentHeapSlots = classDecl.Extends?.Target.HeapSlots;
                var heapSlots = new Slots<VariableDeclaration>(parentHeapSlots);
                foreach (var field in classDecl.Body?.Members.OfType<VariableDeclaration>() ?? new List<VariableDeclaration>())
                {
                    heapSlots.Add(field);
                    field.HeapSlot = heapSlots.GetSlot(field).Value;
                }

                classDecl.HeapSlots = heapSlots;
            }
        }

        private int GetInheritanceLevel(Class c)
        {
            int i = 0;
            while ((c = c.Extends?.Target)!= null)
                i++;
            return i;
        }
    }
}
