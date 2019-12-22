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

        I_CALL = 10,
        I_VCALL,

        I_JMP,
        I_JZ,

        I_SET_L = 20, // priskiria reiksme lokaliam kitamajam stacke
        I_SET_H, // priskiria reiksme objekto kitamajam heape

        I_GET_LA, // get local address

        I_GET_L,
        I_GET_H,
        I_GET_C, // gets current object address 

        I_PUSH, 
        //I_INT_PUSH,
        I_INT_POP,
        //I_FLOAT_PUSH,
        I_FLOAT_POP,
        
        I_INT_ADD = 30,
        I_INT_SUB,
        I_INT_MUL,
        I_INT_DIV,
        I_INT_LESS,
        I_INT_LESSEQ,
        I_INT_GR,
        I_INT_GREQ,

        I_FLOAT_ADD = 40,
        I_FLOAT_SUB,
        I_FLOAT_MUL,
        I_FLOAT_DIV,
        I_FLOAT_LESS,
        I_FLOAT_LESSEQ,
        I_FLOAT_GR,
        I_FLOAT_GREQ,

        I_NEG = 50,
        I_NOT,
        I_AND,
        I_OR,
        I_EQ,
        I_NEQ,

        I_EXIT = 99,

        //for disassembler only
        I_BEGIN_VTABLE = 1000
    }
}
