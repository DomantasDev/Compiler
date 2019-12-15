using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractSyntaxTree_Implementation.CodeGeneration
{
    public enum Instr
    {
        I_ALLOC_H = 1, //kuria objekta heape
        I_ALLOC_S, //alokuoja atminty
        I_CALL_BEGIN,
        I_RET,
        I_RETV,
        I_VCALL,

        I_JMP,
        I_JZ,

        I_SET_L, // priskiria reiksme lokaliam kitamajam stacke
        I_SET_H, // priskiria reiksme objekto kitamajam heape

        I_GET_LA,

        I_INT_PUSH,
        I_INT_POP,
        I_FLOAT_PUSH,
        I_FLOAT_POP,

    }
}
