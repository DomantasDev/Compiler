namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers
{
    public class Parameter : Node
    {
        public TokenNode Type { get; set; }
        public TokenNode Name { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(Type), Type);
            p.Print(nameof(Name), Name);
        }
    }
}