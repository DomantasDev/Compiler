using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.CodeGeneration;
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

        public Label StartLabel { get; } = new Label();
        public Label EndLabel { get; } = new Label();

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

        public override void GenerateCode(CodeWriter w)
        {
            w.PlaceLabel(StartLabel);
            Condition.GenerateCode(w);
            w.Write(Instr.I_JZ, EndLabel);
            Body.GenerateCode(w);
            w.Write(Instr.I_JMP, StartLabel);
        }
    }
}
