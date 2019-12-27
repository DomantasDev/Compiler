using System.Collections.Generic;

namespace CodeGeneration.CodeGeneration
{
    public class Label
    {
        public List<int> Offsets { get; set; }
        public int? Value { get; set; }

        public Label()
        {
            Offsets = new List<int>();
        }
    }
}
