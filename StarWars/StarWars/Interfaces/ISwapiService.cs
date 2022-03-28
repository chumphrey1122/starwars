using StarWars.Models;

namespace StarWars.Interfaces
{
    /// <summary>
    /// Represents a service to get information from the Star Wars API (SWAPI)
    /// </summary>
    public interface ISwapiService
    {
        Task<List<Person>> GetPeopleByNameAsync(string name, CancellationToken cancellationToken);

        Task<List<Planet>> GetPlanetsAsync(CancellationToken cancellationToken);
    }
}
