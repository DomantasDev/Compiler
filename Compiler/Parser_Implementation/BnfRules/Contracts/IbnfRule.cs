using AbstractSyntaxTree_Implementation.Nodes;

namespace Parser_Implementation.BnfRules.Contracts
{
    public interface IBnfRule
    {
        ExpectResult Expect();
    }
}
