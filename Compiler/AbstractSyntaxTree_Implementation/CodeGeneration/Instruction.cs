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

        public static Dictionary<Instr, Instruction> InstructionsByCode { get; set; }
        //public static Dictionary<string, Instruction> InstructionsByName { get; set; }

        static Instruction()
        {
            AddInstruction(Instr.I_ALLOC_H, 1);
            AddInstruction(Instr.I_CALL_BEGIN, 0);
            AddInstruction(Instr.I_VCALL, 2);

            AddInstruction(Instr.I_FLOAT_PUSH, 1);
            AddInstruction(Instr.I_INT_PUSH, 1);
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
