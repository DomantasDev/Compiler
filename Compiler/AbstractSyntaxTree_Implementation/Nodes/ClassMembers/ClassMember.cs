using AbstractSyntaxTree_Implementation.ResolveNames;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers
{
    public abstract class ClassMember : Node
    {
        public abstract void AddName(Scope scope);
    }
}