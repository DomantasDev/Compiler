﻿using System;
using System.Collections.Generic;
using System.Text;
using Lexer_Implementation.DynamicLexer;

namespace AbstractSyntaxTree_Implementation.Nodes
{
    public class LexemeNode : Node
    {
        public Lexeme Value { get; set; }
    }
}
