using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AbstractSyntaxTree_Implementation.Nodes;

namespace AbstractSyntaxTree_Implementation
{
    public class NodePrinter
    {
        private int _indent = 0;
        private string Prefix => new string(' ', _indent * 3);
        public void Print(string title, IEnumerable<Node> nodes)
        {
            if (nodes == null)
            {
                PrintText(title, "null");
                return;
            }

            int i = 0;
            foreach (var node in nodes)
            {
                Print($"{title}[{i++}]", node);
            }
        }
        public void Print(string title, Node node)
        {
            switch (node)
            {
                case null:
                    PrintText(title, "null");
                    break;
                case ITokenNode x:
                    PrintTokenNode(title, x);
                    break;
                default:
                    PrintNode(title, node);
                    break;
            }
        }

        private void PrintNode(string title, Node node)
        {
            PrintText(title, GetName(node));
            _indent++;
            node.Print(this);
            _indent--;
        }
        private void PrintTokenNode(string title, ITokenNode node)
        {
            PrintText($"{title}: {GetName(node)}", $"{node.Value}  {{line: {node.Line}}}");
        }
        private void PrintText(string title, string text = null)
        {
            Console.WriteLine($"{Prefix}{title}: {text ?? string.Empty}");
        }

        private string GetName(object node)
        {
            return node.GetType().ToString().Split('.').TakeLast(1).FirstOrDefault();
        }
    }
}
