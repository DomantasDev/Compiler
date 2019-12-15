using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractSyntaxTree_Implementation.CodeGeneration
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
