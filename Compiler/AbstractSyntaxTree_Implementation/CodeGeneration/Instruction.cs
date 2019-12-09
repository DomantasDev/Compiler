using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractSyntaxTree_Implementation.CodeGeneration
{
    public class Instruction
    {
        public int Code { get; private set; }
        public string Name { get; private set; }
        public int NumOps { get; private set; }

        public static Dictionary<int, Instruction> InstructionsByCode { get; set; }
        public static Dictionary<string, Instruction> InstructionsByName { get; set; }

        static Instruction()
        {
            AddInstruction(0x10, "I_ADD", 0 ); // pasidaryt enuma
        }

        public static void AddInstruction(int code, string name, int numOps)
        {
            var instr = new Instruction
            {
                Code = code,
                Name = name,
                NumOps = numOps
            };

            InstructionsByCode.Add(code, instr);
            InstructionsByName.Add(name, instr);
        }
    }
}
