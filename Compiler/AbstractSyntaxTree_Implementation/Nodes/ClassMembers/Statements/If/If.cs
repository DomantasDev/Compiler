using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions;
using AbstractSyntaxTree_Implementation.ResolveNames;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;
using ValueType = AbstractSyntaxTree_Implementation.Nodes.Types.ValueType;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements.If
{
    public class If : Statement
    {
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
            new ValueType { Value = "bool", Line = conditionType.Line}.IsEqual(conditionType);

            Body?.CheckTypes();
            Else?.CheckTypes();

            return null;
        }
    }
}
