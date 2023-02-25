namespace CatFacts.Exceptions;

public sealed class CatFactRequestFailedException : Exception
{
    public CatFactRequestFailedException(string message) : base(message)
    {
    }
}