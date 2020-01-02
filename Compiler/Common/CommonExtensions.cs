using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class CommonExtensions
    {
        private static readonly Dictionary<char, char> _escapeChars = new Dictionary<char, char>
        {
            {'\\', '\\'}, //   \\ -> \
            {'n', '\n'},  //   \n -> new line
            {'t', '\t'},  //    \t -> tab
            {'r','\r'},  //    \r -> car. ret.
            {'\"', '\"'},  //   \" -> "
        };
        public static string ReplaceWithEscapeChars(this string s)
        {
            var res = string.Empty;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '\\')
                {
                    res += _escapeChars[s[++i]];
                }
                else
                    res += s[i];
            }
            return res;
        }
    }
}
