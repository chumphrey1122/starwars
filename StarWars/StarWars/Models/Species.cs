namespace StarWars.Models
{
    /// <summary>
    /// Represents a species
    /// </summary>
    public class Species
    {
        /// <summary>
        /// The full name of the species
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The taxonomic classification of the species 
        /// </summary>
        public string Classification { get; set; }

        /// <summary>
        /// A list of the Ids of films in which this species is seen
        /// </summary>
        public List<int> FilmIds { get; set; }
    }
}
