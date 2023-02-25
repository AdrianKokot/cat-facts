namespace CatFacts.Entities;

public sealed record CatFact(string Fact, int Length)
{
    public override string ToString()
    {
        return Fact;
    }
}