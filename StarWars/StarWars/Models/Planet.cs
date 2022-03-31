namespace StarWars.Models
{
    /// <summary>
    /// Represents a planet
    /// </summary>
    public class Planet
    {
        /// <summary>
        /// The full name of the planet
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Population of the planet, or null if unknown
        /// </summary>
        public long? Population { get; set; }
    }
}
