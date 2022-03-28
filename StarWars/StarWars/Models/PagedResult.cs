namespace StarWars.Models
{
    public class PagedResult<T>
    {
        public long Count { get; set; }

        public string Next { get; set; }

        public T[] Results { get; set; }
    }
}
