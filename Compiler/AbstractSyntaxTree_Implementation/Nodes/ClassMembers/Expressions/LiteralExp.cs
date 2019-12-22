using System;
using AbstractSyntaxTree_Implementation.CodeGeneration;
using AbstractSyntaxTree_Implementation.Nodes.Types;
using AbstractSyntaxTree_Implementation.ResolveNames;
using Common;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;
using ValueType = AbstractSyntaxTree_Implementation.Nodes.Types.ValueType;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions
{
    public class LiteralExp : Expression, ITokenNode
    {
        public string TokenType { get; set; }
        public string Value { get; set; }
        public int Line { get; set; }

        public override void ResolveNames(Scope scope)
        {
        }

        public override Type CheckTypes()
        {
            return new ValueType
            {
                Value = TokenType.ToLower(),
                Line = Line
            };
        }

        public override void GenerateCode(CodeWriter w)
        {
            w.Write(Instr.I_PUSH, GetInt());
        }

        private int GetInt()
        {
            switch (TokenType)
            {
                case "INT":
                    return int.Parse(Value);
                case "FLOAT":
                    return BitConverter.ToInt32(BitConverter.GetBytes(float.Parse(Value)));
                case "BOOL":
                    return Value == "true" ? 1 : 0;
                //TODO string?
                default:
                    throw new Exception($"{nameof(LiteralExp)}".UnexpectedError(Line));
            }
        }
    }
}
