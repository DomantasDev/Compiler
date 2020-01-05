using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeExecution
{
    public class MemoryAllocator
    {
        private readonly int[] _memory;
        private readonly int _offset;
        public const int headerSize = 2;

        private int Start
        {
            get => _memory[_offset]; 
            set => _memory[_offset] = value;
        }

        public MemoryAllocator(int[] memory, int offset = 4096, int heapSize = 4096)
        {
            _memory = memory;
            _offset = offset;

            _memory[_offset] = _offset + 1;

            _memory[_offset + 1] = 0; // set next
            _memory[_offset + 2] = heapSize;  //set size
        }

        public int Allocate(int requestedSize)
        {
            int blockStart = Start;
            int previousBlockStart = _offset;
            int blockSize;

            while (blockStart != 0)
            {
                blockSize = _memory.BlockSize(blockStart);

                if (blockSize > requestedSize + headerSize + 1)
                {
                    _memory.SetBlockSize(blockStart, requestedSize);

                    var newBlockStart = blockStart + requestedSize + headerSize;
                    _memory.SetNextBlockAddress(previousBlockStart, newBlockStart);

                    _memory.SetNextBlockAddress(newBlockStart, _memory.NextBlockAddress(blockStart));
                    _memory.SetBlockSize(newBlockStart, blockSize - requestedSize - headerSize);

                    return blockStart + headerSize;
                }

                if (blockSize >= requestedSize && blockSize <= requestedSize + headerSize + 1) // requestedSize <= blocksize <= requestedSize + 3
                {
                    _memory[previousBlockStart] = _memory.NextBlockAddress(blockStart);
                    return blockStart + headerSize; // reikia +2, nes reikia zinoti kiek baitu trinti
                }

                previousBlockStart = blockStart;
                blockStart = _memory.NextBlockAddress(blockStart);

            } 

            return 0; // failed to allocate memory
        }

        public void Delete(int addressToDelete)
        {
            int leftNeighbor = 0;
            int rightNeighbor = 0;

            int blockToDelete = addressToDelete - headerSize;
            int blockStart = Start;

            while (blockStart != 0)
            {
                if (blockStart < blockToDelete)
                {
                    leftNeighbor = blockStart;
                }

                if (blockStart > blockToDelete)
                {
                    rightNeighbor = blockStart;
                    break;
                }

                blockStart = _memory.NextBlockAddress(blockStart);
            }

            if (leftNeighbor == 0)
            {
                _memory.SetNextBlockAddress(blockToDelete, rightNeighbor);
                Start = blockToDelete;

                _memory.MergeBlocks(blockToDelete, rightNeighbor);
                
            }
            else
            {
                _memory.SetNextBlockAddress(leftNeighbor, blockToDelete);
                _memory.SetNextBlockAddress(blockToDelete, rightNeighbor);

                _memory.MergeBlocks(blockToDelete, rightNeighbor);
                _memory.MergeBlocks(leftNeighbor, blockToDelete);
            }

        }
    }

    public static class MemoryExtensions
    {
        public static void MergeBlocks(this int[] memory, int leftBlockStart, int rightBlockStart)
        {
            if(!memory.BlocksAreAdjacent(leftBlockStart, rightBlockStart))
                return;

            var rightBlockSize = memory.BlockSize(rightBlockStart);
            var leftBlockSize = memory.BlockSize(leftBlockStart);
            memory.SetBlockSize(leftBlockStart, leftBlockSize + rightBlockSize + MemoryAllocator.headerSize);
            var newRightNeighbor = memory.NextBlockAddress(rightBlockStart);
            memory.SetNextBlockAddress(leftBlockStart, newRightNeighbor);
        }
        public static bool BlocksAreAdjacent(this int[] memory, int leftBlockStart, int rightBlockStart)
        {
            return leftBlockStart + MemoryAllocator.headerSize + memory.BlockSize(leftBlockStart) == rightBlockStart;
        }
        public static int NextBlockAddress(this int[] memory, int blockStart)
        {
            return memory[blockStart];
        }

        public static void SetNextBlockAddress(this int[] memory, int blockStart, int address)
        {
            memory[blockStart] = address;
        }

        public static int BlockSize(this int[] memory, int blockStart)
        {
            return memory[blockStart + 1];
        }

        public static void SetBlockSize(this int[] memory, int blockStart, int size)
        {
            memory[blockStart + 1] = size;
        }
    }
}
