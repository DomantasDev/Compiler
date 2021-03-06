﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Lexer_Implementation.DynamicLexer.FSM
{
    internal class RecursionState
    {
        internal State FirstState { get; set; }
        internal HashSet<char> FirstReccursionChars { get; set; } = new HashSet<char>();
        internal bool CreatedNewStateOnFirstStep { get; set; }
        internal string RecursionName { get; set; }


        internal RecursionState Clone()
        {
            return new RecursionState
            {
                CreatedNewStateOnFirstStep = CreatedNewStateOnFirstStep,
                FirstState = FirstState,
                RecursionName = RecursionName,
                FirstReccursionChars = FirstReccursionChars
            };
        }
    }
}
