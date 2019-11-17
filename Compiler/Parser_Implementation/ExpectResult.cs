using System;
using System.Collections.Generic;
using System.Text;
using AbstractSyntaxTree_Implementation.Nodes;

namespace Parser_Implementation
{
    public class ExpectResult
    {
        public ExpectResult(bool success, Node node) : this(success)
        {
            Node = node;
        }

        public ExpectResult(Node node)
        {
            Node = node;
        }
        public ExpectResult(bool success)
        {
            Success = success;
        }
        public ExpectResult() { }
        public bool Success { get; set; }
        public Node Node { get; set; }
    }
}
