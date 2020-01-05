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
        private bool _repetitionSucceeded = false;

        public BnfRuleAlternative(List<IBnfRule> bnfRules, LexemeSource lexemeSource, List<Repetition> repetitions, MetaData metaData) : base(bnfRules, lexemeSource, metaData)
        {
            _repetitions = repetitions;
        }

        public override ExpectResult Expect()
        {
            _repetitionSucceeded = false;
            return MetaData?.Class == null ? ExpectNode() : CreateNode();
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

                var temp = _repetitionSucceeded; 
                var result = BnfRules[i].Expect();
                _repetitionSucceeded = temp;

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

            if (_repetitions.Any() && _repetitionSucceeded && MetaData.IsLeftRecursion) //jei yra repetition, ir repetition kazka rado, ir pazymeta kairioji rekursija
            {
                return new ExpectResult(true, LeftRecursion(results));
            }
            if (_repetitions.Any() && !_repetitionSucceeded && MetaData.IsLeftRecursion) //jei yra repetition, ir repetition nieko nerado, ir pazymeta kairioji rekursija
            {
                return results.First().Item;
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

                if (parameters.Count == 2 && parameters[1] == null && MetaData.Class == "Assign")
                {

                }
                newNode = NodeFactory.CreateNode(MetaData.Class, parameters);
            }
            else
            {
                newNode = NodeFactory.CreateNode(MetaData.Class, paramGroups);
            }

            return new ExpectResult(true, newNode);
        }

        private Node LeftRecursion(List<IndexedItem<ExpectResult>> expectResults)
        {
            Node result = null;
            var paramsNeeded = MetaData.ParamGroups.Count;
            var nodes = expectResults.Select(x => x.Item.Node).ToList();
            while (nodes.Count > 1)
            {
                var paramsForFactory = nodes
                    .Take(paramsNeeded)
                    .ToList();

                nodes = nodes
                    .Skip(paramsNeeded)
                    .ToList();

                result = NodeFactory.CreateNode(MetaData.Class, paramsForFactory);

                nodes.Insert(0, result);
            }

            return result;
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

                _repetitionSucceeded = true;
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

            if (MetaData != null)
            {
                var firstParamGroup = MetaData.ParamGroups.FirstOrDefault()?.Params;

                if (firstParamGroup.Any())
                {
                    return results[firstParamGroup.First()]; // return @x
                }

                return new ExpectResult(true); // return null if @
            }

            return results[0];
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
