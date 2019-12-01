using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes;

namespace AbstractSyntaxTree_Implementation.ResolveNames
{
    public class Scope
    {
        public static Dictionary<string, Node> ClassTable = new Dictionary<string, Node>();
        public readonly Scope ParentScope;
        //public List<Scope> Children { get; set; }
        public string Name { get; set; }
        private readonly Dictionary<Name, Node> _members;

        public Scope(Scope parentScope, string name = null)
        {
            ParentScope = parentScope;
            Name = name;
            _members = new Dictionary<Name, Node>();
        }

        public void Add(Name name, Node node)
        {
            if(!_members.TryAdd(name, node))
                Console.WriteLine($"Duplicate name: {name}");
        }

        public Node ResolveName(Name name)
        {
            Node result;
            if (_members.TryGetValue(name, out result))
                return result;

            if(ParentScope != null)
                return ParentScope?.ResolveName(name);

            Console.WriteLine($"UndeclaredVariable: {name.Value}, line: {name.Line}");

            return null;
        }

        //public Node ResolveMember(Name className, Name memberName)
        //{
        //    var scope = this;
        //    Scope classScope;

        //    while (scope != null)
        //    {
        //        if()
        //    }
        //}

    }
}
