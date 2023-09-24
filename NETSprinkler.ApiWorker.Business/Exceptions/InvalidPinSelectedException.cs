namespace NETSprinkler.ApiWorker.Business.Exceptions;

public class InvalidPinSelectedException : Exception
{
    public InvalidPinSelectedException(string msg) : base(msg)
    {
    }
}