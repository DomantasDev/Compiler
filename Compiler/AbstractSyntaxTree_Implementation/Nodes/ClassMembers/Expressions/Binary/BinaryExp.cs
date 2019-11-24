namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions.Binary
{
    public class BinaryExp : Expression
    {
        public Expression Left { get; set; }
        public TokenNode Operator { get; set; }
        public Expression Right { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(Left), Left);
            p.Print(nameof(Operator), Operator);
            p.Print(nameof(Right), Right);
        }
    }
}
