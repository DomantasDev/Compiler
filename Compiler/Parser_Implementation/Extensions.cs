using System;
using System.Collections.Generic;
using System.Linq;
using AbstractSyntaxTree_Implementation.Nodes;
using Lexer_Contracts;

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

        public static TokenNode ToNode(this Token token)
        {
            return new TokenNode
            {
                TokenType = token.Type,
                Value = token.Value,
                Line = token.Line
            };
        }

        public static (List<Token> data, MetaData metaData) ExtractMetaData(this List<Token> lexemes)
        {
            var x = lexemes.Aggregate(
                (data: new List<Token>(), metaData: new List<Token>()),
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
                metaData = new MetaData();

                var firstMetaData = x.metaData.First();

                if (firstMetaData.Type == "META_RECURSION")
                {
                    metaData.IsLeftRecursion = true;
                    x.metaData = x.metaData.Skip(1).ToList();
                    firstMetaData = x.metaData.First();
                }

                if (firstMetaData.Type == "META_CLASS")
                {
                    metaData.Class = firstMetaData.Value.Substring(1, firstMetaData.Value.Length - 2);
                    x.metaData = x.metaData.Skip(1).ToList();
                }

                metaData.ParamGroups = GetParamGroups(x.metaData);

            }

            return (x.data, metaData);
        }

        private static List<ParamGroup> GetParamGroups(List<Token> lexemes)
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
