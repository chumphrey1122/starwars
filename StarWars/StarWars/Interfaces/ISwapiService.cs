using StarWars.Models;

namespace StarWars.Interfaces
{
    /// <summary>
    /// Represents a service to get information from the Star Wars API (SWAPI). If the Star Wars API changes
    /// its definition downstream, if possible you should change the implementation of this interface without 
    /// changing the interface itself
    /// </summary>
    public interface ISwapiService
    {
        Task<IEnumerable<Person>> GetPeopleByNameAsync(string name, CancellationToken cancellationToken);

        Task<IEnumerable<Planet>> GetPlanetsAsync(CancellationToken cancellationToken);

        Task<IEnumerable<Starship>> GetStarshipsForPerson(Person person, CancellationToken cancellationToken);

        Task<IEnumerable<Species>> GetSpecies(CancellationToken cancellationToken);
    }
}
