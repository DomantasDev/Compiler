﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers;
using Common;

namespace AbstractSyntaxTree_Implementation.ResolveNames
{
    public class Scope
    {
        public static Dictionary<string, Class> ClassTable = new Dictionary<string, Class>();
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
                $"Duplicate name: \"{name.Value}\"".RaiseError(name.Line);
        }

        public Node ResolveName(Name name, bool suppressError = false)
        {
            Node result;
            if (_members.TryGetValue(name, out result))
                return result;

            if(ParentScope != null)
                return ParentScope?.ResolveName(name, suppressError);

            if(!suppressError)
                $"Undeclared variable: \"{name.Value}\"".RaiseError(name.Line);

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

        public static Node ResolveForClass(string className, Name memberName)
        {
            var classNode = ClassTable[className];
            while (classNode != null)
            {
                var member = classNode.Scope.ResolveName(memberName, true);
                if (member != null)
                    return member;

                classNode = classNode.Extends?.TargetClass;
            }

            $"Undeclared variable: \"{memberName.Value}\"".RaiseError(memberName.Line);

            return null;
        }

    }
}
