namespace StarWars.Models
{
    /// <summary>
    /// Represents a person
    /// </summary>
    public class Person
    {
        /// <summary>
        /// The full name of the person
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A list of URLs that point to Starships associated with this person
        /// </summary>
        public string[] StarshipReferences { get; set; }
    }
}
