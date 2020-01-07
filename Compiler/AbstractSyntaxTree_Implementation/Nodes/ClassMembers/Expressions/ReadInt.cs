using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes.Types;
using AbstractSyntaxTree_Implementation.ResolveNames;
using CodeGeneration.CodeGeneration;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;
using ValueType = AbstractSyntaxTree_Implementation.Nodes.Types.ValueType;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions
{
    public class ReadInt : Expression, ITokenNode
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
                Line = Line,
                Value = "int"
            };

            return Type;
        }

        public override void GenerateCode(CodeWriter w)
        {
            w.Write(Instr.I_READ, Line);
        }

    }
}
