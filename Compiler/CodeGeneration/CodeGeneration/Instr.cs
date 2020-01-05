namespace CodeGeneration.CodeGeneration
{
    public enum Instr
    {
        I_ALLOC_H = 1, //kuria objekta heape
        I_ALLOC_HS, // alloc string in sheap
        I_DEL, // deletes obj in heap

        I_ALLOC_S, //alokuoja atminty stacke
        I_CALL_BEGIN,
        I_RET,
        I_RETV,

        I_CALL = 10,
        I_VCALL,

        I_JMP,
        I_JZ,

        I_SET_L = 20, // priskiria reiksme lokaliam kitamajam stacke
        I_SET_H, // priskiria reiksme objekto kitamajam heape
        I_SET_A, // sets specified address

        I_GET_LA, // get local address

        I_GET_L = 25,
        I_GET_H,
        I_GET_C, // gets current object address (push(HFP))

        I_PUSH, 
        //I_INT_PUSH,
        //I_INT_POP,
        //I_FLOAT_PUSH,
        //I_FLOAT_POP,
        
        I_INT_ADD = 30,
        I_INT_SUB,
        I_INT_MUL,
        I_INT_DIV,
        I_INT_LESS,
        I_INT_LESSEQ,
        I_INT_GR,
        I_INT_GREQ,
        I_INT_NEG,

        I_FLOAT_ADD = 40,
        I_FLOAT_SUB,
        I_FLOAT_MUL,
        I_FLOAT_DIV,
        I_FLOAT_LESS,
        I_FLOAT_LESSEQ,
        I_FLOAT_GR,
        I_FLOAT_GREQ,
        I_FLOAT_NEG,

        I_NOT = 50,
        I_AND,
        I_OR,
        I_EQ,
        I_NEQ,

        I_EXIT = 99,

        I_WRITE = 100,
        I_READ,
        I_SLEEP,
        I_RAND_INT,
        I_CLEAR,
        I_GET_KEY,

        //for disassembler only
        I_BEGIN_VTABLE = 1000
    }
}
