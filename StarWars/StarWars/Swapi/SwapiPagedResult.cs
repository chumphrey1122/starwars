namespace StarWars.Swapi
{
    /// <summary>
    /// Represents a collection of objects returned by the SWAPI APIs
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SwapiPagedResult<T>
    {
        public long Count { get; set; }

        public string Next { get; set; }

        public T[] Results { get; set; }
    }
}
