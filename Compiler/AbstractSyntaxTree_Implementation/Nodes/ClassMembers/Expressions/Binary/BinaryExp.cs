namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions.Binary
{
    public abstract class BinaryExp : Expression
    {
        public virtual Expression Left { get; set; }
        public virtual TokenNode Operator { get; set; }
        public virtual Expression Right { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(Left), Left);
            p.Print(nameof(Operator), Operator);
            p.Print(nameof(Right), Right);
        }
    }
}
