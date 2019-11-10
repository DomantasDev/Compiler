using System;
using System.Collections.Generic;
using System.Text;
using Parser_Implementation.BnfRules;
using Parser_Implementation.BnfRules.Contracts;

namespace Parser_Implementation
{
    public static class Extensions
    {
        public static bool Expect(this IEnumerable<IBnfRule> rules)
        {
            foreach (var bnfRule in rules)
            {
                if (!bnfRule.Expect())
                    return false;
            }

            return true;
        }
    }
}
