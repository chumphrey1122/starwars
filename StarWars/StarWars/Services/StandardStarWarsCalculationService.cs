using StarWars.Exceptions;
using StarWars.Interfaces;
using StarWars.Models;
using System.Linq;

namespace StarWars.Services
{
    /// <summary>
    /// This service uses the <see cref="ISwapiService"/> to pull data from SWAPI and then 
    /// performs calculations on it.
    /// 
    /// TODO: Since the SWAPI data does not change much in real time, I should implement 
    /// caching of all relevant data that are returned. This will improve performance and 
    /// scalability of the application. 
    /// </summary>
    public class StandardStarWarsCalculationService : IStarWarsCalculationService
    {
        private ISwapiService _swapiService;

        public StandardStarWarsCalculationService(ISwapiService swapiService)
        {
            _swapiService = swapiService;
        }

        public async Task<IEnumerable<SpeciesClassification>> GetSpeciesClassificationByMovie(int movieId, CancellationToken cancellationToken)
        {
            var species = await _swapiService.GetSpecies(cancellationToken);
            if (movieId < 1 || movieId > 6)
                throw new ResourceNotFoundException();

            return species.
                Where(x=>x.FilmIds.Contains(movieId)).
                Select(species => new SpeciesClassification() { Classification = species.Classification, Name = species.Name});
        }

        public async Task<IEnumerable<Starship>> GetStarshipsForPilot(string pilotName, CancellationToken cancellationToken)
        {
            var pilots = (await _swapiService.GetPeopleByNameAsync(pilotName, cancellationToken)).ToList();
            if (!pilots.Any())
                throw new ResourceNotFoundException(); // no pilot found
            if (pilots.Count != 1)
                throw new SearchReturnedMultipleResultsException(); // Too many pilots found
            return await _swapiService.GetStarshipsForPerson(pilots.First(), cancellationToken);
        }

        public async Task<long> GetTotalPopulation(CancellationToken cancellationToken)
        {
            var planets = await _swapiService.GetPlanetsAsync(cancellationToken);
            return planets.Sum(planet => planet.Population ?? 0);
        }
    }
}
