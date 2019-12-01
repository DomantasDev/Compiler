using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions;
using AbstractSyntaxTree_Implementation.ResolveNames;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;
using ValueType = AbstractSyntaxTree_Implementation.Nodes.Types.ValueType;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements
{
    public class Loop : Statement
    {
        public Expression Condition { get; set; }
        public Body Body { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(Condition), Condition);
            p.Print(nameof(Body), Body);
        }

        public override void ResolveNames(Scope scope)
        {
            Condition.ResolveNames(scope);
            Body?.ResolveNames(scope);
        }

        public override Type CheckTypes()
        {
            new ValueType{Value = "bool"}.IsEqual(Condition.CheckTypes());
            
            Body?.CheckTypes();

            return null;
        }
    }
}
