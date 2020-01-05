using AbstractSyntaxTree_Implementation.Nodes.Types;
using AbstractSyntaxTree_Implementation.ResolveNames;
using CodeGeneration.CodeGeneration;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions
{
    public class GetKey : Expression, ITokenNode
    {
        public string TokenType { get; set; }
        public string Value { get; set; }
        public int Line { get; set; }
        public override void ResolveNames(Scope scope)
        {
        }

        public override Type CheckTypes()
        {
            Type = new ValueType
            {
                Value = "int",
                Line = Line
            };

            return Type;
        }

        public override void GenerateCode(CodeWriter w)
        {
            w.Write(Instr.I_GET_KEY);
        }
    }
}
