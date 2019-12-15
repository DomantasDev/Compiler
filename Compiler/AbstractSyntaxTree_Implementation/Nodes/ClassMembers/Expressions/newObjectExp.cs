using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes.Types;
using AbstractSyntaxTree_Implementation.ResolveNames;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions
{
    public class NewObjectExp : Expression
    {
        public List<Expression> Arguments { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(Type), Type);
            p.Print(nameof(Arguments), Arguments);
        }

        public override void ResolveNames(Scope scope)
        {
            Type.ResolveNames(scope);
            Arguments?.ForEach(x => x.ResolveNames(scope));
        }

        public override Type CheckTypes()
        {
            //TODO make constructors parameterless
            Arguments?.ForEach(x => x.CheckTypes());
            return Type;
        }
    }
}
