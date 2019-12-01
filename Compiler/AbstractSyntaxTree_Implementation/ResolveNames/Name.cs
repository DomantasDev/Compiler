using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes;

namespace AbstractSyntaxTree_Implementation.ResolveNames
{
    public class Name
    {
        private readonly ITokenNode _node;

        public string Value => _node.Value;
        public int Line => _node.Line;
        public NameType Type { get; }

        public Name(ITokenNode node, NameType type)
        {
            _node = node;
            Type = type;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode() ^ Type.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Name;
            if (other == null)
                return false;

            return Value == other.Value && Type == other.Type;
        }
    }
}
