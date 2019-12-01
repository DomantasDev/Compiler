using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes;

namespace AbstractSyntaxTree_Implementation
{
    public static class Extensions
    {
        public static void CheckLength<T>(this List<T> nodes, int expectedLength)
        {
            if(nodes.Count != expectedLength)
                throw new Exception("Kazkas negerai");
        }

        public static string CreateConstructorName(string className, IEnumerable<string> paramTypes)
        {
            paramTypes = paramTypes ?? new List<string>();
            
            return $"<{className}.{string.Join('.', paramTypes)}>";
        }
    }
}
