namespace StarWars.Models
{
    /// <summary>
    /// A starship
    /// </summary>
    public class Starship
    {
        /// <summary>
        /// The full name of the starship
        /// </summary>
        public string Name { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }

        /// <summary>
        /// The cost of the starship in credits
        /// </summary>
        public decimal? CostInCredits { get; set; }
        public double? Length { get; set; }
        public double? MaxAtmospheringSpeed { get; set; }

        /// <summary>
        /// The number of crew the starship can take, or null if unknown.
        /// </summary>
        public int? Crew { get; set; }
        public int? Passengers { get; set; }
        public double? CargoCapacity { get; set; }
        public string Consumables { get; set; }
        public double? HyperdriveRating { get; set; }
        public int? MGLT { get; set; }
        public string StarshipClass { get; set; }
    }
}
