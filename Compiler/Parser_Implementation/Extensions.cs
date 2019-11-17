using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lexer_Implementation.DynamicLexer;
using Microsoft.Win32.SafeHandles;
using Parser_Implementation.BnfRules;
using Parser_Implementation.BnfRules.Contracts;

namespace Parser_Implementation
{
    public static class Extensions
    {
        //public static bool Expect(this IEnumerable<IBnfRule> rules)
        //{
        //    foreach (var bnfRule in rules)
        //    {
        //        if (!bnfRule.Expect())
        //            return false;
        //    }

        //    return true;
        //}

        public static (List<Lexeme> data, MetaData metaData) ExtractMetaData(this List<Lexeme> lexemes)
        {
            var x = lexemes.Aggregate(
                (data: new List<Lexeme>(), metaData: new List<Lexeme>()),
                (res, lexeme) =>
                {
                    if (lexeme.Type.StartsWith("META"))
                        res.metaData.Add(lexeme);
                    else
                        res.data.Add(lexeme);
                    return res;
                });

            MetaData metaData = null;
            if (x.metaData.Any())
            {
                var classMetaData = x.metaData.First();

                metaData = new MetaData
                {
                    Class = classMetaData.Type == "META_CLASS" ? classMetaData.Value.Substring(1, classMetaData.Value.Length - 2) : throw new Exception("ismok rasyt BNF"),
                    ParamGroups = GetParamGroups(x.metaData.Skip(1).ToList())
                };
            }

            return (x.data, metaData);
        }

        private static List<ParamGroup> GetParamGroups(List<Lexeme> lexemes)
        {
            if(lexemes.First().Type != "META_SEPARATOR")
                throw new Exception("ismok rasyt BNF");

            var result = new List<ParamGroup>();
            var newGroup = new ParamGroup();

            foreach (var lexeme in lexemes.Skip(1))
            {
                if(lexeme.Type == "META_GROUPING")
                {
                    newGroup.Params.Add(int.Parse(lexeme.Value));
                }
                else if (lexeme.Type == "META_SEPARATOR")
                {
                    result.Add(newGroup);
                    newGroup = new ParamGroup();
                }
                else
                {
                    throw new Exception("nope...");
                }
            }

            result.Add(newGroup);

            return result;
        }
    }
}
