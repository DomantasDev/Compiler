using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbstractSyntaxTree_Implementation.CodeGeneration
{
    public class CodeWriter
    {
        private readonly List<int> _code = new List<int>{0};

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

        public void Write(Label x)
        {
            _code.Add(x.Value.Value);
        }

        //public void Write(Instr instrType, List<Label> ops = null)
        //{
        //    var instruction = Instruction.InstructionsByCode[instrType];

        //    if(instruction.NumOps != (ops?.Count ?? 0))
        //        throw new Exception("dafuq r u doin");

        //    _code.Add(instruction.Code);
        //    ops?.ForEach(AddOp);
        //}

        //public void Write(Instr instrType, params Label[] ops)
        //{
        //    Write(instrType, ops.ToList());
        //}

        //public void Write(Instr instrType, params int[] ops)
        //{
        //    var instruction = Instruction.InstructionsByCode[instrType];
        //    _code.Add(instruction.Code);
        //    foreach (var op in ops)
        //    {
        //        _code.Add(op);
        //    }
        //}

        public void Write(Instr instrType, params object[] ops)
        {
            var instruction = Instruction.InstructionsByCode[instrType];
            _code.Add(instruction.Code);
            foreach (var op in ops)
            {
                AddOp(op);
            }
        }

        public void Disassemble()
        {
            Console.WriteLine(string.Join(',', _code));

            int offset = 1;
            while (offset < _code.Count)
            {
                var opCode = _code[offset];
                var instr = Instruction.InstructionsByCode[(Instr) opCode];
                var ops = _code.Skip(offset + 1).Take(instr.NumOps).ToList();
                Console.WriteLine($"{offset.ToString().PadRight(4)}:\t{instr.Name.PadRight(10)}  {string.Join(',', ops)}");
                offset += 1 + ops.Count;
            }
        }

        private void AddOp(object operand)
        {
            if (operand is Label op)
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
            else if (operand is int x)
            {
                _code.Add(x);
            }
        }
    }
}
