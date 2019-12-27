using System;
using System.Globalization;
using AbstractSyntaxTree_Implementation.ResolveNames;
using CodeGeneration.CodeGeneration;
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
            Type =  new ValueType
            {
                Value = TokenType.ToLower(),
                Line = Line
            };

            return Type;
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
                    return BitConverter.ToInt32(BitConverter.GetBytes(float.Parse(Value, CultureInfo.InvariantCulture)));
                case "BOOL":
                    return Value == "true" ? 1 : 0;
                //TODO string?
                default:
                    throw new Exception($"{nameof(LiteralExp)}".UnexpectedError(Line));
            }
        }
    }
}
