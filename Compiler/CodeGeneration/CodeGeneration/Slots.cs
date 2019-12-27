using System.Collections.Generic;
using System.Linq;

namespace CodeGeneration.CodeGeneration
{
    public class Slots<T>
    {
        private readonly Slots<T> _parent;

        public Slots(Slots<T> parent, int startingIndex = 0)
        {
            _parent = parent;
            NextSlot = parent?.NextSlot ?? startingIndex;
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

        public Dictionary<int, T> GetSlots()
        {
            
            if (_parent == null)
            {
                return _slots.ToDictionary(x => x.Value, x => x.Key);
            }

            var parentSlots = _parent.GetSlots();
            foreach (var slot in _slots)
            {
                parentSlots[slot.Value] = slot.Key;
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
