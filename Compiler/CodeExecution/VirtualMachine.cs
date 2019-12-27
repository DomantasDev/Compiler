using System;
using System.Collections.Generic;
using CodeGeneration.CodeGeneration;
using Common;

namespace CodeExecution
{
    public class VirtualMachine
    {
        private readonly int[] _code = new int[4096];
        private int IP;
        private int SP;
        private int SFP;
        private int HFP;
        private int HP;
        private bool _running;

        public VirtualMachine(int[] code)
        {
            code.CopyTo(_code,0);
            IP = 1;
            SP = 1024;
            SFP = 1024;
            HP = 2048;
            HFP = 2048;
            _running = true;
        }

        public void Execute()
        {
            while (_running)
            {
                ExecuteOne();
            }

            Console.WriteLine($"ProgramResult = {Pop()}");
        }

        private void ExecuteOne()
        {

            var cmd = Read();
            switch ((Instr)cmd)
            {
                case Instr.I_INT_ADD:
                    BinaryOp(Pop, (a, b) => a + b, Push);
                    break;
                case Instr.I_INT_SUB:
                    BinaryOp(Pop, (a, b) => a - b, Push);
                    break;
                case Instr.I_INT_MUL:
                    BinaryOp(Pop, (a, b) => a * b, Push);
                    break;
                case Instr.I_INT_DIV:
                    BinaryOp(Pop, (a, b) => a / b, Push);
                    break;
                case Instr.I_INT_LESS:
                    BinaryOp(Pop, (a, b) => a < b, PushBool);
                    break;
                case Instr.I_INT_LESSEQ:
                    BinaryOp(Pop, (a, b) => a <= b, PushBool);
                    break;
                case Instr.I_INT_GR:
                    BinaryOp(Pop, (a, b) => a > b, PushBool);
                    break;
                case Instr.I_INT_GREQ:
                    BinaryOp(Pop, (a, b) => a >= b, PushBool);
                    break;                
                case Instr.I_FLOAT_ADD:
                    BinaryOp(Pop, (a, b) => a + b, Push);
                    break;
                case Instr.I_FLOAT_SUB:
                    BinaryOp(PopFloat, (a, b) => a - b, PushFLoat);
                    break;
                case Instr.I_FLOAT_MUL:
                    BinaryOp(PopFloat, (a, b) => a * b, PushFLoat);
                    break;
                case Instr.I_FLOAT_DIV:
                    BinaryOp(PopFloat, (a, b) => a / b, PushFLoat);
                    break;
                case Instr.I_FLOAT_LESS:
                    BinaryOp(PopFloat, (a, b) => a < b, PushBool);
                    break;
                case Instr.I_FLOAT_LESSEQ:
                    BinaryOp(PopFloat, (a, b) => a <= b, PushBool);
                    break;
                case Instr.I_FLOAT_GR:
                    BinaryOp(PopFloat, (a, b) => a > b, PushBool);
                    break;
                case Instr.I_FLOAT_GREQ:
                    BinaryOp(PopFloat, (a, b) => a >= b, PushBool);
                    break;
                case Instr.I_AND:
                    BinaryOp(PopBool, (a, b) => a && b, PushBool);
                    break;
                case Instr.I_OR:
                    BinaryOp(PopBool, (a, b) => a || b, PushBool);
                    break;
                case Instr.I_EQ:
                    BinaryOp(Pop, (a, b) => a == b, PushBool);
                    break;
                case Instr.I_NEQ:
                    BinaryOp(Pop, (a, b) => a != b, PushBool);
                    break;
                case Instr.I_NOT:
                    UnaryOp(PopBool, a => !a, PushBool);
                    break;
                case Instr.I_INT_NEG:
                    UnaryOp(Pop, a => -a, Push);
                    break;
                case Instr.I_FLOAT_NEG:
                    UnaryOp(PopFloat, a => -a, PushFLoat);
                    break;
                case Instr.I_PUSH:
                    Push(Read());
                    break;
                case Instr.I_JMP:
                    IP = Read();
                    break;
                case Instr.I_JZ:
                    var condition = Pop();
                    var newIp = Read();
                    if (condition == 0)
                        IP = newIp;
                    break;
                case Instr.I_ALLOC_S:
                    SP += Read();
                    break;
                case Instr.I_ALLOC_H:
                    AllocHeap(Read(), Read());
                    break;
                case Instr.I_CALL_BEGIN:
                    SP += 5;
                    break;
                case Instr.I_VCALL:
                    VCall(Read(), Read());
                    break;
                case Instr.I_RETV:
                    Return(Pop());
                    break;
                case Instr.I_RET:
                    Return(0);
                    break;
                case Instr.I_SET_L:
                    SetLocal(Read());
                    break;
                case Instr.I_SET_H:
                    SetHeap(Read());
                    break;
                case Instr.I_SET_A:
                    SetAddress();
                    break;
                case Instr.I_GET_C:
                    Push(HFP);
                    break;
                case Instr.I_GET_L:
                    Push(_code[SFP + Read()]);
                    break;
                case Instr.I_GET_H:
                    Push(_code[Pop() + Read()]);
                    break;



                case Instr.I_EXIT:
                    _running = false;
                    break;
                case Instr.I_BEGIN_VTABLE:
                    break;
                default:
                    throw new Exception($"Unrecognized command {cmd}".RaiseError());
            }

            //Console.WriteLine($"{(Instr)cmd}");
            //Console.WriteLine($"IP = {IP}");
            //Console.WriteLine($"SP = {SP}");
            //Console.WriteLine($"SFP = {SFP}");
            //Console.WriteLine($"HP = {HP}");
            //Console.WriteLine($"HFP = {HFP}");
            //var stackNum = SP - 1024;
            //var heapNum = HP - 2048;
            //Console.WriteLine($"{"Stack".PadLeft(13)}{"Heap:".PadLeft(10)}");
            //for (int i = 0; i < Math.Max(stackNum, heapNum); i++)
            //{
            //    Console.Write($"{i.ToString().PadRight(3)}:");
            //    Console.Write($"{_code[1024 + i].ToString().PadLeft(12)}");
            //    Console.WriteLine($"{_code[2048 + i].ToString().PadLeft(10)}");
            //}
            //Console.WriteLine(new string('-', 30));
        }

        private void SetAddress()
        {
            var address = Pop();
            var value = Pop();
            _code[address] = value;
        }

        private void SetHeap(int slot)
        {
            var value = Pop();
            _code[HFP + slot] = value;
        }

        private void SetLocal(int slot)
        {
            var value = Pop();
            _code[SFP + slot] = value;
        }

        private void Return(int value)
        {
            var tempSFP = SFP;
            IP = _code[tempSFP - 5];
            SP = _code[tempSFP - 4];
            SFP = _code[tempSFP - 3];
            //HP = _code[tempSFP - 2];
            HFP = _code[tempSFP - 1];

            Push(value);
        }
        private void VCall(int methodNumber, int numArgs)
        {
            var objectAddress = Pop();

            _code[SP - numArgs - 5] = IP;    //IP
            _code[SP - numArgs - 4] = SP - numArgs - 5;  // SP
            _code[SP - numArgs - 3] = SFP; // SFP
            _code[SP - numArgs - 2] = HP; // HP
            _code[SP - numArgs - 1] = HFP; // HFP

            
            var vTableAddress = _code[objectAddress];
            var methodAddress = _code[vTableAddress + methodNumber];

            IP = methodAddress;
            SFP = SP - numArgs;
            HFP = objectAddress;
        }

        private void AllocHeap(int vTableAddress, int size)
        {
            _code[HP] = vTableAddress;
            Push(HP);
            HP += size;
        }

        private void BinaryOp<T, RT>(Func<T> pop, Func<T, T, RT> operation, Action<RT> push)
        {
            T right = pop();
            T left = pop();
            var result = operation(left, right);
            push(result);
        }

        private void UnaryOp<T>(Func<T> pop, Func<T, T> operation, Action<T> push)
        {
            T a = pop();
            var result = operation(a);
            push(a);
        }

        //private void NumericOp<T>(NumericOpType opType) where T : struct
        //{
        //    var b = Pop();
        //    var a = Pop();
        //    if (typeof(T) == typeof(int))
        //    {
        //        var left = a;
        //        var right = b;
        //        switch (opType)
        //        {
        //            case
        //        }
        //    }
        //}

        private int Read()
        {
            return _code[IP++];
        }

        private int Pop()
        {
            return _code[--SP];
        }

        private float PopFloat()
        {
            var floatAsInt = Pop();
            float result = BitConverter.Int32BitsToSingle(floatAsInt);
            return result;
        }

        private bool PopBool()
        {
            var boolAsInt = Pop();
            bool result = boolAsInt != 0;
            return result;
        }

        private void Push(int x)
        {
            _code[SP++] = x;
        }

        private void PushFLoat(float x)
        {
            var floatAsInt = BitConverter.ToInt32(BitConverter.GetBytes(x));
            Push(floatAsInt);
        }

        private void PushBool(bool x)
        {
            var boolAsInt = x ? 1 : 0;
            Push(boolAsInt);
        }
    }
}
