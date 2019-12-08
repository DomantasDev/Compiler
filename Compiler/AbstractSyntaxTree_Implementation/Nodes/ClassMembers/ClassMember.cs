using AbstractSyntaxTree_Implementation.CodeGeneration;
using AbstractSyntaxTree_Implementation.ResolveNames;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers
{
    public abstract class ClassMember : Node
    {
        public TokenNode Name { get; set; }
        public Label StartLabel { get; set; } = new Label();
        public abstract void AddName(Scope scope);

        public override int GetHashCode()
        {
            return Name.Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as ClassMember;
            if (other == null)
                return false;

            return Name.Value == other.Name.Value;
        }
    }
}