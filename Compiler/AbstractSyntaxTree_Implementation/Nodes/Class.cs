
namespace AbstractSyntaxTree_Implementation.Nodes
{
    public class Class : Node
    {
        public TokenNode Name { get; set; }
        public TokenNode Extends { get; set; }
        public ClassBody Body { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print("name", Name);
            p.Print("extends",Extends);
            p.Print("Body", Body);
        }
    }
}