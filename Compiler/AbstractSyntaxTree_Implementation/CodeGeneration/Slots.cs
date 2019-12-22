using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly Dictionary<T, int> _slots = new Dictionary<T, int>();
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

        public Dictionary<T, int> GetSlots()
        {
            
            if (_parent == null)
            {
                return new Dictionary<T, int>(_slots);
            }

            var parentSlots = _parent.GetSlots();
            foreach (var slot in _slots)
            {
                parentSlots[slot.Key] = slot.Value;
            }

            return parentSlots;
        }

        public int GetNumSlots()
        {
            var slots = GetSlots();
            return slots.Count;
        }

    }

    public class Slot
    {
        public int Nr { get; set; }
        public Label Label { get; set; }
    }
}
