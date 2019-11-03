using System;
using System.Collections.Generic;
using System.Text;

namespace Lexer_Implementation.DotFileGenerator
{
    public class StateCounter
    {
        private Dictionary<string, int> _map;

        public StateCounter()
        {
            _map = new Dictionary<string, int>();
        }

        public int GetCount(string s)
        {
            if (_map.ContainsKey(s))
            {
                return ++_map[s];
            }
            else
            {
                _map.Add(s, 1);
                return 1;
            }
        }
    }
}
