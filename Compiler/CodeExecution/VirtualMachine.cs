using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading;
using CodeGeneration.CodeGeneration;
using Common;

namespace CodeExecution
{
    public class VirtualMachine
    {
        private const int StackStart = 3000;
        private const int HeapStart = 5000;
        private const int MemorySize = 9000;

        private readonly int[] _memory = new int[MemorySize];
        private int IP;
        public int SP
        {
            get => _SP;
            set
            {
                if (value >= HeapStart || value < StackStart)
                    throw new StackOverflowException();
                _SP = value;
            }
        }

        private int _SP;
        private int SFP;
        private int HFP;
        private int HP;
        private bool _running;

        private Random _random;

        private MemoryAllocator _allocator;

        public VirtualMachine(int[] code)
        {
            code.CopyTo(_memory,0);
            IP = 1;
            SP = StackStart;
            SFP = StackStart;
            HP = HeapStart;
            HFP = HeapStart;
            _running = true;
            _allocator = new MemoryAllocator(_memory,HeapStart, MemorySize - HeapStart);

            _random = new Random(); 
        }

        public void Execute()
        {
            while (_running)
            {
                ExecuteOne();
            }

            //Console.WriteLine(Pop());
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
                case Instr.I_ALLOC_HS:
                    AllocString(Read());
                    break;
                case Instr.I_DEL:
                    Delete();
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
                    if(HFP == 0)
                        throw new NullReferenceException();
                    Push(HFP);
                    break;
                case Instr.I_GET_L:
                    Push(_memory[SFP + Read()]);
                    break;
                case Instr.I_GET_H:
                    GetObjectField();
                    break;

                case Instr.I_WRITE:
                    Write(Read());
                    break;
                case Instr.I_READ:
                    ReadInt();
                    break;
                case Instr.I_SLEEP:
                    Thread.Sleep(Pop());
                    break;
                case Instr.I_RAND_INT:
                    Push(_random.Next(Pop()));
                    break;
                case Instr.I_CLEAR:
                    Console.Clear();
                    break;
                case Instr.I_GET_KEY:
                    GetKey();
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
            //    Console.Write($"{_memory[1024 + i].ToString().PadLeft(12)}");
            //    Console.WriteLine($"{_memory[2048 + i].ToString().PadLeft(10)}");
            //}
            //Console.WriteLine(new string('-', 30));
        }

        private void ReadInt()
        {
            var line = Read();

            var stringValue = Console.ReadLine();
            if (int.TryParse(stringValue, out var intValue))
            {
                Push(intValue);
            }
            else
            {
                "Read value is not int".RaiseError(line);
                throw new ArgumentException("");
            }

        }

        private void GetObjectField()
        {
            var address = Pop();
            if(address == 0)
                throw  new NullReferenceException();
            Push(_memory[address + Read()]);
        }

        private void GetKey()
        {
            //int key = 0;
            //while (Console.KeyAvailable)
            //{
            //    key = (int)Console.ReadKey(true).Key;
            //}

            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                Push((int)key);
                return;
            }

            Push(0);
        }

        private void Write(int numArgs)
        {
            for (int i = 0; i < numArgs; i++)
            {
                PrintArgument((PrimitiveType)Pop());
            }

            void PrintArgument(PrimitiveType prim)
            {
                switch (prim)
                {
                    case PrimitiveType.Int:
                        Console.Write(Pop());
                        break;
                    case PrimitiveType.Float:
                        Console.Write(PopFloat().ToString(CultureInfo.InvariantCulture));
                        break;
                    case PrimitiveType.Bool:
                        Console.Write(PopBool());
                        break;
                    case PrimitiveType.String:
                        PrintString(Pop());
                        break;
                }
            }

            void PrintString(int address)
            {
                string s = string.Empty;
                char c = (char) _memory[address];

                for (int i = 1; c != 0; i++)
                {
                    s += c;
                    c = (char) _memory[address + i];
                }

                Console.Write(s.ReplaceWithEscapeChars());
            }
        }

        private void SetAddress()
        {
            var address = Pop();
            if (address == 0)
                throw new NullReferenceException();
            var value = Pop();
            _memory[address] = value;
        }

        private void SetHeap(int slot)
        {
            var value = Pop();

            if(HFP == 0)
                throw new NullReferenceException();

            _memory[HFP + slot] = value;
        }

        private void SetLocal(int slot)
        {
            var value = Pop();
            _memory[SFP + slot] = value;
        }

        private void Return(int value)
        {
            var tempSFP = SFP;
            IP = _memory[tempSFP - 5];
            SP = _memory[tempSFP - 4];
            SFP = _memory[tempSFP - 3];
            //HP = _code[tempSFP - 2];
            HFP = _memory[tempSFP - 1];

            Push(value);
        }
        private void VCall(int methodNumber, int numArgs)
        {
            var objectAddress = Pop();
            if(objectAddress == 0)
                throw new NullReferenceException();

            _memory[SP - numArgs - 5] = IP;    //IP
            _memory[SP - numArgs - 4] = SP - numArgs - 5;  // SP
            _memory[SP - numArgs - 3] = SFP; // SFP
            _memory[SP - numArgs - 2] = HP; // HP
            _memory[SP - numArgs - 1] = HFP; // HFP

            
            var vTableAddress = _memory[objectAddress];
            var methodAddress = _memory[vTableAddress + methodNumber];

            IP = methodAddress;
            SFP = SP - numArgs;
            HFP = objectAddress;
        }

        private void AllocHeap(int vTableAddress, int size)
        {
            //_code[HP] = vTableAddress;
            //Push(HP);
            //HP += size;


            var address = _allocator.Allocate(size);
            _memory[address] = vTableAddress;
            Push(address);
            if (size == 7)
            {

            }
        }
        private void AllocString(int stringLength)
        {
            var address = _allocator.Allocate(stringLength + 1);
            for (int i = 0; i < stringLength; i++)
            {
                _memory[address + i] = Read();
            }

            _memory[address + stringLength + 1] = 0;
            Push(address);
        }

        private void Delete()
        {
            var address = Pop();
            if(address == 0)
                throw new NullReferenceException();
            _allocator.Delete(address);
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
            push(result);
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
            return _memory[IP++];
        }

        private int Pop()
        {
            return _memory[--SP];
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
            _memory[SP++] = x;
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
