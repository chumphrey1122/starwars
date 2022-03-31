namespace StarWars.Exceptions
{
    /// <summary>
    /// This exception is returned when a search returns more than one result, but
    /// only 1 result is expected.
    /// </summary>
    public class SearchReturnedMultipleResultsException : Exception
    {
    }
}
