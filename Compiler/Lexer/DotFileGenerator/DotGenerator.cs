using Lexer_Implementation.DynamicLexer.FSM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lexer_Implementation.DotFileGenerator
{
    public class DotGenerator
    {
        private readonly DotGeneratorHelper _helper;
        private readonly StateNameResolver _nameResolver;
        private readonly HashSet<State> _visitedStates;

        public DotGenerator(DotGeneratorHelper helper, StateNameResolver nameResolver)
        {
            _helper = helper;
            _nameResolver = nameResolver;
            _visitedStates = new HashSet<State>();
        }
        public void GenerateDot(State startingState, string fileName)
        {
            AddState(startingState);
            File.WriteAllText(fileName, _helper.DotString);
        }

        private void AddState(State state)
        {
            if (!_visitedStates.Add(state))
                return;
            var from = _nameResolver.ResolveName(state);
            var transitions = state.Transitions
                .Select((s, i) => new { State = s, Index = i})
                .Where(t => t.State != null)
                .GroupBy(
                    x => x.State
                );
            foreach (var x in transitions)
            {
                var endState = x.Key;
                var to = _nameResolver.ResolveName(endState);
                var label = string.Join(',', x.Select(s => (char)s.Index));
                var nodeShape = endState.IsFinal ? NodeShape.DoubleCircle : NodeShape.Circle;
                _helper.Transition(from, to, label, nodeShape);
                AddState(endState);
            }
        }
    }

    public class DotGeneratorHelper
    {
        private NodeShape _currentNodeShape;
        private StringBuilder _dotStringBuilder = new StringBuilder();

        public string DotString { get => WrapUp(_dotStringBuilder.ToString()); }

        public void Transition(string from, string to, string label, NodeShape nodeShape)
        {
            if (_currentNodeShape != nodeShape)
            {
                _dotStringBuilder.Append($"\tnode [shape = {nodeShape.ToString().ToLower()}];\n");
                _currentNodeShape = nodeShape;
            }
            _dotStringBuilder.Append($"\t{from} -> {to} [ label = \"{label.ReplaceEscapeChars()}\" ];\n");
        }

        private string WrapUp(string dotString)
        {
            return $@"digraph finite_state_machine {{
                rankdir = LR; 
                size = ""8,5""
                { dotString }}}";
        }
    }

    static class Extensions
    {
        private static readonly (string oldValue, string newValue)[] _escapeChars = new (string, string)[]
        {
            ("\\", "\\\\"),  //   \n -> new line
            ("\n", "\\n"),  //   \n -> new line
            ("\t", "\\t"),  //    \t -> tab
            ("\r","\\r"),  //    \r -> car. ret.
            ("\"", "\\\""),  //   \" -> "
        };
        internal static string ReplaceEscapeChars(this string s)
        {
            foreach (var x in _escapeChars)
                s = s.Replace(x.oldValue, x.newValue);
            return s;
        }
    }
}
