using System.Collections.Generic;
using System.Linq;
using AbstractSyntaxTree_Implementation.Nodes;
using Lexer_Implementation.DynamicLexer;
using Parser_Implementation.BnfRules.Contracts;
using Parser_Implementation.Lexemes;

namespace Parser_Implementation.BnfRules.Alternatives
{
    public class BnfRuleAlternative : BnfRuleBase
    {
        private readonly List<Repetition> _repetitions;


        public BnfRuleAlternative(List<IBnfRule> bnfRules, LexemeSource lexemeSource, List<Repetition> repetitions, MetaData metaData) : base(bnfRules, lexemeSource, metaData)
        {
            _repetitions = repetitions;
        }

        public override ExpectResult Expect()
        {
            return MetaData == null ? ExpectNode() : CreateNode();
        }

        private ExpectResult CreateNode()
        {
            var results = new List<IndexedItem<ExpectResult>>(BnfRules.Count);
            var checkpoint = LexemeSource.SetCheckpoint();

            for (var i = 0; i < BnfRules.Count; i++)
            {
                Repetition rep;
                if (_repetitions.Any() && (rep = _repetitions.FirstOrDefault(r => r.StartIndex == i)) != null)
                {
                    results.AddRange(Repetition(rep));
                    i = rep.EndIndex;
                    continue;
                }

                var result = BnfRules[i].Expect();
                if (!result.Success)
                {
                    LexemeSource.RevertCheckPoint(checkpoint);
                    return new ExpectResult(false);
                }

                if (MetaData.ParamGroups.SelectMany(p => p.Params).Contains(i))
                    results.Add(new IndexedItem<ExpectResult>
                    {
                        Index = i,
                        Item = result
                    });
            }

            var paramGroups = new List<List<Node>>(MetaData.ParamGroups.Count);
            for (var i = 0; i < MetaData.ParamGroups.Count; i++)
            {
                paramGroups.Add(new List<Node>());
            }

            foreach (var r in results)
            {
                for (var i = 0; i < MetaData.ParamGroups.Count; i++)
                {
                    if (MetaData.ParamGroups[i].Params.Contains(r.Index))
                    {
                        paramGroups[i].Add(r.Item.Node);
                        break;
                    }
                }
            }

            Node newNode;
            if (!_repetitions.Any() && paramGroups.All(p => p.Count <= 1))
            {
                var parameters = paramGroups.Select(p => p.FirstOrDefault()).ToList();
                newNode = NodeFactory.CreateNode(MetaData.Class, parameters);
            }
            else
            {
                newNode = NodeFactory.CreateNode(MetaData.Class, paramGroups);
            }

            return new ExpectResult(true, newNode);
        }

        private List<IndexedItem<ExpectResult>> Repetition(Repetition repetition)
        {
            var result = new List<IndexedItem<ExpectResult>>();

            bool failed = false;
            while (true)
            {
                var checkpoint = LexemeSource.SetCheckpoint();

                var temp = new List<IndexedItem<ExpectResult>>();
                for (var i = repetition.StartIndex; i <= repetition.EndIndex; i++)
                {
                    var expectRes = BnfRules[i].Expect();
                    if (!expectRes.Success)
                    {
                        failed = true;
                        break;
                    }

                    if (MetaData.ParamGroups.SelectMany(p => p.Params).Contains(i))
                    {
                        temp.Add(new IndexedItem<ExpectResult>
                        {
                            Index = i,
                            Item = expectRes
                        });
                    }
                }

                if (failed)
                {
                    LexemeSource.RevertCheckPoint(checkpoint);
                    break;
                }
                result.AddRange(temp);
            }

            return result;
        }

        private ExpectResult ExpectNode()
        {
            var results = new List<ExpectResult>(BnfRules.Count);
            var checkpoint = LexemeSource.SetCheckpoint();

            for (var i = 0; i < BnfRules.Count; i++)
            {
                Repetition rep;
                if (_repetitions != null && _repetitions.Any() && (rep = _repetitions.FirstOrDefault(r => r.StartIndex == i)) != null)
                {
                    results.AddRange(RepetitionWhenExpecting(rep).Select(r => r.Item));
                    i = rep.EndIndex;
                    continue;
                }

                var bnfRule = BnfRules[i];
                var res = bnfRule.Expect();
                if (!res.Success)
                {
                    LexemeSource.RevertCheckPoint(checkpoint);
                    return new ExpectResult(false);
                }

                results.Add(res);
            }

            return results.First(r => r.Success && r.Node != null); //TODO change back to single
        }

        private List<IndexedItem<ExpectResult>> RepetitionWhenExpecting(Repetition repetition)
        {
            var result = new List<IndexedItem<ExpectResult>>();

            bool failed = false;
            while (true)
            {
                var checkpoint = LexemeSource.SetCheckpoint();

                var temp = new List<IndexedItem<ExpectResult>>();
                for (var i = repetition.StartIndex; i <= repetition.EndIndex; i++)
                {
                    var expectRes = BnfRules[i].Expect();
                    if (!expectRes.Success)
                    {
                        failed = true;
                        break;
                    }

                    temp.Add(new IndexedItem<ExpectResult>
                    {
                        Index = i,
                        Item = expectRes
                    });
                    
                }

                if (failed)
                {
                    LexemeSource.RevertCheckPoint(checkpoint);
                    break;
                }
                result.AddRange(temp);
            }

            return result;
        }
    }
}
