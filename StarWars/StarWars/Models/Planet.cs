namespace StarWars.Models
{
    public class Planet
    {
        public string Name { get; set; }

        public string Population { get; set; }

        public long PopulationAsLong()
        {
            return long.TryParse(Population, out var population) ? population : 0;
        }
    }
}
