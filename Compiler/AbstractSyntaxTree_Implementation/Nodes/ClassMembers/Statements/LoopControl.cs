using AbstractSyntaxTree_Implementation.ResolveNames;
using CodeGeneration.CodeGeneration;
using Common;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements
{
    public class LoopControl : Statement, ITokenNode
    {
        public string TokenType { get; set; }
        public string Value { get; set; }
        public int Line { get; set; }

        public override void ResolveNames(Scope scope)
        {
            var ancestor = FindAncestor<Loop>();
            if(ancestor == null)
                $"{Value} not in a loop".RaiseError(Line);
        }

        public override Type CheckTypes()
        {
            return null;
        }

        public override void GenerateCode(CodeWriter w)
        {
            var loop = FindAncestor<Loop>();
            switch (TokenType)
            {
                case "KW_BREAK":
                    w.Write(Instr.I_JMP, loop.EndLabel);
                    break;
                case "KW_CONTINUE":
                    w.Write(Instr.I_JMP, loop.StartLabel);
                    break;
            }
            
        }
    }
}
