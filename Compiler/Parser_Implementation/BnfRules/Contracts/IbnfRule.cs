namespace Parser_Implementation.BnfRules.Contracts
{
    public interface IBnfRule
    {
        bool Expect();
        string RuleName { get; }
    }
}
