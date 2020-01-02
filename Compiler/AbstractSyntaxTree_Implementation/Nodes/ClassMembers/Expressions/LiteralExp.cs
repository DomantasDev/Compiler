using System;
using System.Globalization;
using System.Linq;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements.If;
using AbstractSyntaxTree_Implementation.Nodes.Types;
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
            if (TokenType == "STRING")
            {
                Type = new StringType
                {
                    Value = TokenType.ToLower(),
                    Line = Line
                };
            }
            else
            {
                Type = new ValueType
                {
                    Value = TokenType.ToLower(),
                    Line = Line
                };
            }
            

            return Type;
        }

        public override void GenerateCode(CodeWriter w)
        {
            if (TokenType == "STRING")
            {
                w.Write(Instr.I_ALLOC_HS, Value.Length - 2);
                var ints = Value.Skip(1).SkipLast(1).Select(x => (int) x);
                foreach (var i in ints)
                {
                    w.Write(i);
                }
            }
            else
            {
                w.Write(Instr.I_PUSH, GetInt());
            }
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

                default:
                    throw new Exception($"{nameof(LiteralExp)}".UnexpectedError(Line));
            }
        }
    }
}
