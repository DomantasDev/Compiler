﻿using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Expressions;
using AbstractSyntaxTree_Implementation.ResolveNames;
using Type = AbstractSyntaxTree_Implementation.Nodes.Types.Type;

namespace AbstractSyntaxTree_Implementation.Nodes.ClassMembers.Statements.IO
{
    public class Write : Statement
    {
        public List<Expression> Arguments { get; set; }

        public override void Print(NodePrinter p)
        {
            p.Print(nameof(Arguments), Arguments);
        }

        public override void ResolveNames(Scope scope)
        {
            Arguments.ForEach(x => x.ResolveNames(scope));
        }

        public override Type CheckTypes()
        {
            return null;
        }
    }
}
