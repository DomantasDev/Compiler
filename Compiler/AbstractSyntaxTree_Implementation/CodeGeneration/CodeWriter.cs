using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbstractSyntaxTree_Implementation.CodeGeneration
{
    public class CodeWriter
    {
        private List<int> _code;

        public Label NewLabel()
        {
            return new Label();
        }

        public void CompleteLabel(Label label, int value)
        {
            label.Value = value;
            label.Offsets.ForEach(x => _code[x] = value);
        }

        public void PlaceLabel(Label label)
        {
            CompleteLabel(label, _code.Count);
        }

        public void Write(Instr instrType, List<Label> ops = null)
        {
            var instruction = Instruction.InstructionsByCode[instrType];

            _code.Add(instruction.Code);
            ops.ForEach(AddOp);
        }

        public void Write(Instr instrType, params Label[] ops)
        {
            Write(instrType, ops.ToList());
        }

        public void Write(Instr instrType, params int[] ops)
        {
            var instruction = Instruction.InstructionsByCode[instrType];
            _code.Add(instruction.Code);
            foreach (var op in ops)
            {
                _code.Add(op);
            }
        }

        private void AddOp(Label op)
        {
            if (op.Value.HasValue)
            {
                _code.Add(op.Value.Value);
            }
            else
            {
                op.Offsets.Add(_code.Count);
                _code.Add(9999);
            }
        }
    }
}
