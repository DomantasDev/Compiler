using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractSyntaxTree_Implementation.CodeGeneration
{
    public class Instruction
    {
        private Instr InstructionType { get; set; }
        public string Name => InstructionType.ToString();

        public int Code => (int)InstructionType;
        public int NumOps { get; private set; }

        public static Dictionary<Instr, Instruction> InstructionsByCode { get; set; } = new Dictionary<Instr, Instruction>();
        //public static Dictionary<string, Instruction> InstructionsByName { get; set; }

        static Instruction()
        {
            AddInstruction(Instr.I_PUSH, 1);

            AddInstruction(Instr.I_ALLOC_H, 2); // vtable address, num of fields
            AddInstruction(Instr.I_ALLOC_S, 1);

            AddInstruction(Instr.I_CALL_BEGIN, 0);
            AddInstruction(Instr.I_VCALL, 2);

            AddInstruction(Instr.I_JMP, 1);
            AddInstruction(Instr.I_JZ, 1);

            AddInstruction(Instr.I_RET,0);
            AddInstruction(Instr.I_RETV,0);

            AddInstruction(Instr.I_GET_L,1);
            AddInstruction(Instr.I_GET_H,1);
            AddInstruction(Instr.I_GET_C,0);

            AddInstruction(Instr.I_SET_L, 1);
            AddInstruction(Instr.I_SET_H,1);

            AddInstruction(Instr.I_INT_ADD,0);
            AddInstruction(Instr.I_INT_SUB,0);
            AddInstruction(Instr.I_INT_MUL,0);
            AddInstruction(Instr.I_INT_DIV,0);
            AddInstruction(Instr.I_INT_LESS,0);
            AddInstruction(Instr.I_INT_LESSEQ, 0);
            AddInstruction(Instr.I_INT_GR, 0);
            AddInstruction(Instr.I_INT_GREQ, 0);

            AddInstruction(Instr.I_FLOAT_ADD, 0);
            AddInstruction(Instr.I_FLOAT_SUB, 0);
            AddInstruction(Instr.I_FLOAT_MUL, 0);
            AddInstruction(Instr.I_FLOAT_DIV, 0);
            AddInstruction(Instr.I_FLOAT_LESS, 0);
            AddInstruction(Instr.I_FLOAT_LESSEQ, 0);
            AddInstruction(Instr.I_FLOAT_GR, 0);
            AddInstruction(Instr.I_FLOAT_GREQ, 0);

            AddInstruction(Instr.I_NEG,0);
            AddInstruction(Instr.I_NOT,0);
            AddInstruction(Instr.I_OR, 0);
            AddInstruction(Instr.I_AND, 0);
            AddInstruction(Instr.I_EQ, 0);
            AddInstruction(Instr.I_NEQ, 0);

            AddInstruction(Instr.I_EXIT, 0);
            //AddInstruction(Instr.I_FLOAT_PUSH, 1);
            //AddInstruction(Instr.I_INT_PUSH, 1);
        }

        public static void AddInstruction(Instr instructionType, int numOps)
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
