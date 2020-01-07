using System.Collections.Generic;

namespace CodeGeneration.CodeGeneration
{
    public class Instruction
    {
        public Instr InstructionType { get; set; }
        public string Name => InstructionType.ToString();

        public int Code => (int)InstructionType;
        public int NumOps { get; private set; }

        public static Dictionary<Instr, Instruction> InstructionsByCode { get; set; } = new Dictionary<Instr, Instruction>();
        //public static Dictionary<string, Instruction> InstructionsByName { get; set; }

        static Instruction()
        {
            AddInstruction(Instr.I_PUSH, 1);

            AddInstruction(Instr.I_ALLOC_H, 2); // vTable address, num of fields
            AddInstruction(Instr.I_ALLOC_HS, 1); // string length, reads the string
            AddInstruction(Instr.I_DEL);

            AddInstruction(Instr.I_ALLOC_S, 1);

            AddInstruction(Instr.I_CALL_BEGIN);
            AddInstruction(Instr.I_VCALL, 2); // method number, num args

            AddInstruction(Instr.I_JMP, 1);
            AddInstruction(Instr.I_JZ, 1);

            AddInstruction(Instr.I_RET);
            AddInstruction(Instr.I_RETV);

            AddInstruction(Instr.I_GET_L,1);
            AddInstruction(Instr.I_GET_H,1); // heapslot, pops obj address
            AddInstruction(Instr.I_GET_C);

            AddInstruction(Instr.I_SET_L, 1);
            AddInstruction(Instr.I_SET_H,1);
            AddInstruction(Instr.I_SET_A); // pops address, value

            AddInstruction(Instr.I_INT_ADD);
            AddInstruction(Instr.I_INT_SUB);
            AddInstruction(Instr.I_INT_MUL);
            AddInstruction(Instr.I_INT_DIV);
            AddInstruction(Instr.I_INT_LESS);
            AddInstruction(Instr.I_INT_LESSEQ);
            AddInstruction(Instr.I_INT_GR);
            AddInstruction(Instr.I_INT_GREQ);
            AddInstruction(Instr.I_INT_NEG);

            AddInstruction(Instr.I_FLOAT_ADD);
            AddInstruction(Instr.I_FLOAT_SUB);
            AddInstruction(Instr.I_FLOAT_MUL);
            AddInstruction(Instr.I_FLOAT_DIV);
            AddInstruction(Instr.I_FLOAT_LESS);
            AddInstruction(Instr.I_FLOAT_LESSEQ);
            AddInstruction(Instr.I_FLOAT_GR);
            AddInstruction(Instr.I_FLOAT_GREQ);
            AddInstruction(Instr.I_FLOAT_NEG);


            AddInstruction(Instr.I_NOT);
            AddInstruction(Instr.I_OR);
            AddInstruction(Instr.I_AND);
            AddInstruction(Instr.I_EQ);
            AddInstruction(Instr.I_NEQ);

            AddInstruction(Instr.I_WRITE, 1); // num args
            AddInstruction(Instr.I_READ, 1); // code line
            AddInstruction(Instr.I_SLEEP); // pops sleep time in ms
            AddInstruction(Instr.I_RAND_INT); // pops max value
            AddInstruction(Instr.I_CLEAR);
            AddInstruction(Instr.I_GET_KEY); 

            AddInstruction(Instr.I_EXIT);
            //AddInstruction(Instr.I_FLOAT_PUSH, 1);
            //AddInstruction(Instr.I_INT_PUSH, 1);

            //for disassembler only
            AddInstruction(Instr.I_BEGIN_VTABLE);
        }

        public static void AddInstruction(Instr instructionType, int numOps = 0)
        {
            var instr = new Instruction
            {
                InstructionType = instructionType,
                NumOps = numOps
            };

            InstructionsByCode.Add(instructionType, instr);
            //InstructionsByName.Add(name, instr);
        }
    }
}
