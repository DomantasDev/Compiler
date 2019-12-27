using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions;
using AbstractSyntaxTree_Implementation.ResolveNames;
using CodeGeneration.CodeGeneration;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;
using ValueType = AbstractSyntaxTree_Implementation.Nodes.Types.ValueType;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements.If
{
    public class If : Statement
    {
        public TokenNode KwIf { get; set; }
        public Expression Condition { get; set; }
        public Body Body { get; set; }
        public Else Else { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(Condition), Condition);
            p.Print(nameof(Body), Body);
            p.Print(nameof(Else), Else);
        }

        public override void ResolveNames(Scope scope)
        {
            Condition.ResolveNames(scope);
            Body?.ResolveNames(new Scope(scope));
            Else?.ResolveNames(scope);
        }

        public override Type CheckTypes()
        {
            var conditionType = Condition.CheckTypes();
            new ValueType { Value = "bool", Line = KwIf.Line}.IsEqual(conditionType);

            Body?.CheckTypes();
            Else?.CheckTypes();

            return null;
        }

        public override void GenerateCode(CodeWriter w)
        {
            var next = w.NewLabel();
            var end = w.NewLabel();

            Condition.GenerateCode(w); 
            w.Write(Instr.I_JZ, next);
            Body?.GenerateCode(w);
            w.Write(Instr.I_JMP, end);
            w.PlaceLabel(next);
            Else?.GenerateCode(w);
            w.PlaceLabel(end);
        }
    }
}
