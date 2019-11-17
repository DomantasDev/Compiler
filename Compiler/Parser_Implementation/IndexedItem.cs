using System;
using System.Collections.Generic;
using System.Text;

namespace Parser_Implementation
{
    public class IndexedItem<T>
    {
        public int Index { get; set; }
        public T Item { get; set; }
    }
}
