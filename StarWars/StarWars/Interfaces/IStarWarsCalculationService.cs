using StarWars.Models;

namespace StarWars.Interfaces
{
    /// <summary>
    /// This represents a service that gets the Star Wars data we want. 
    /// This service intentionally does not directly talk to SWAPI (and instead uses another service to do that) 
    /// so that we can change it in future to obtain information from different APIs as well without breaking the 
    /// interface, and to aid with unit testing
    /// </summary>
    public interface IStarWarsCalculationService
    {
        /// <summary>
        /// Get the total population of all known planets
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<long> GetTotalPopulation(CancellationToken cancellationToken);

        /// <summary>
        /// Get the species in the specified movie
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns></returns>
        Task<IEnumerable<SpeciesClassification>> GetSpeciesClassificationByMovie(int movieId, CancellationToken cancellationToken);

        /// <summary>
        /// Returns a list of Starships for the specified pilot
        /// </summary>
        /// <param name="pilotName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<Starship>> GetStarshipsForPilot(string pilotName, CancellationToken cancellationToken);
    }
}
