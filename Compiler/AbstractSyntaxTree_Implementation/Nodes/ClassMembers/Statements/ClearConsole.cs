using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.ResolveNames;
using CodeGeneration.CodeGeneration;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements
{
    class ClearConsole : Statement, ITokenNode
    {
        public string TokenType { get; set; }
        public string Value { get; set; }
        public int Line { get; set; }

        public override void ResolveNames(Scope scope)
        {

        }

        public override Type CheckTypes()
        {
            return null;
        }

        public override void GenerateCode(CodeWriter w)
        {
            w.Write(Instr.I_CLEAR);
        }
    }
}
