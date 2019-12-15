using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.ResolveNames;
using Lexer_Contracts;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions
{
    public class VariableExp : Expression, ITokenNode
    {
        public Node Target { get; set; }
        public string TokenType { get; set; }
        public string Value { get; set; }
        public int Line { get; set; }

        public override void ResolveNames(Scope scope)
        {
            Target = scope.ResolveName(new Name(this, NameType.Variable));
        }

        public override Type CheckTypes()
        {
            return (Type)Target?.GetType().GetProperty("Type")?.GetMethod.Invoke(Target, null);
        }
    }
}
