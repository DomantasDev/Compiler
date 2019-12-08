using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes;

namespace AbstractSyntaxTree_Implementation.CodeGeneration
{
    public class Slots<T> where T : Node
    {
        private readonly Slots<T> _parent;

        public Slots(Slots<T> parent)
        {
            _parent = parent;
        }
        private Dictionary<T, int> _slots;
        public int NextSlot { get; private set; }

        public void Add(T node)
        {
            var slot = GetSlot(node) ?? NextSlot++;

            _slots.Add(node, slot);
        }

        public int? GetSlot(T node)
        {
            if(_slots.TryGetValue(node, out int slot))
                return slot;
            
            return _parent?.GetSlot(node);
        }

    }
}
