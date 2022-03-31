using StarWars.Models;

namespace StarWars.Swapi
{
    /// <summary>
    /// Represents a person on the SWAPI interface
    /// This is intentionally different from the <see cref="Person"/> class (and does not inherit from it) so that, 
    /// if the SWAPI specification changes in future, we do not need to change the ISwapiService interface.
    /// </summary>
    public class SwapiPerson
    {
        public string Name { get; set; }

        public string[] Starships { get; set; }
    }
}
