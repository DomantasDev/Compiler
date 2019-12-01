using System;
using System.Linq;
using AbstractSyntaxTree_Implementation.Nodes.Types;
using AbstractSyntaxTree_Implementation.ResolveNames;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions.Binary.Dot
{
    public abstract class MemberExp : BinaryExp
    {
        public override void ResolveNames(Scope scope)
        {
            Left.ResolveNames(scope);
        }
    }
}
