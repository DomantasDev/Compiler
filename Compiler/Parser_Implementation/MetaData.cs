using System;
using System.Collections.Generic;
using System.Text;

namespace Parser_Implementation
{
    public class MetaData
    {
        public string Class { get; set; }
        public List<ParamGroup> ParamGroups { get; set; }
        public bool IsLeftRecursion { get; set; }
    }

    public class ParamGroup
    {
        public ParamGroup()
        {
            Params = new List<int>();
        }

        public List<int> Params { get; set; }
    }
}
