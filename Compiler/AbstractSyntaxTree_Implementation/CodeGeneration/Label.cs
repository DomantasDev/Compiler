using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractSyntaxTree_Implementation.CodeGeneration
{
    public class Label
    {
        public List<int> Offsets { get; set; }
        public string Name { get; set; }
    }
}
