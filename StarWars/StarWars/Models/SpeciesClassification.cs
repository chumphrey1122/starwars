namespace StarWars.Models
{
    /// <summary>
    /// This simple model represents the classification of a species. 
    /// Note that we do not make <see cref="Species"/> inherit from this because we want to be
    /// sure that when we pass a SpeciesClassification object to be serialized, we want to 
    /// make sure it's not actually a <see cref="Species"/> object, which would introduce 
    /// additional properties we don't need in our API response.
    /// </summary>
    public class SpeciesClassification
    {
        /// <summary>
        /// The full name of the species
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The taxonomic classification of the species 
        /// </summary>
        public string Classification { get; set; }
    }
}
