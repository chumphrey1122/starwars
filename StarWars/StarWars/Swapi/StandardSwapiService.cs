using StarWars.Interfaces;
using System.Linq;
using StarWars.Models;
using System.Web;

namespace StarWars.Swapi
{
    /// <summary>
    /// Concrete implementation of the ISwapiService that communicates with the SWAPI Api, and converts all
    /// returned data to a standard form that the rest of the application can understand.
    /// </summary>
    public class StandardSwapiService : ISwapiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<StandardSwapiService> _logger;

        private HttpClient GetClient()
        {
            return _httpClientFactory.CreateClient("swapi");
        }

        public StandardSwapiService(IHttpClientFactory httpClientFactory, ILogger<StandardSwapiService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        #region Convert to non-Swapi types
        /// <summary>
        /// Convert the SwapiPerson object, which is used for communication with the Swapi API, into an object
        /// that can be used within the main application. This allows us to support changes to the SWAPI api
        /// schema without having to rewrite the entire application (just this service).
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        private static Person ToPerson(SwapiPerson person)
        {
            return new Person()
            {
                Name = person.Name,
                StarshipReferences = person.Starships,
            };
        }

        private static Planet ToPlanet(SwapiPlanet planet)
        {
            return new Planet()
            {
                Name = planet.Name,
                Population = long.TryParse(planet.Population, out var population) ? population : null,
            };
        }

        private static Starship ToStarship(SwapiStarship starship)
        {
            return new Starship()
            {
                CargoCapacity = double.TryParse(starship.CargoCapacity, out double cargoCapacity) ? cargoCapacity : null,
                Consumables = starship.Consumables,
                CostInCredits = decimal.TryParse(starship.CostInCredits, out decimal costInCredits) ? costInCredits : null,
                Crew = int.TryParse(starship.Crew, out int crew) ? crew : null,
                HyperdriveRating = double.TryParse(starship.HyperdriveRating, out double hyperdriveRating) ? hyperdriveRating : null,
                Length = double.TryParse(starship.Length, out double length) ? length : null,
                Manufacturer = starship.Manufacturer,
                MaxAtmospheringSpeed = double.TryParse(starship.MaxAtmospheringSpeed, out double maxSpeed) ? maxSpeed : null,
                MGLT = int.TryParse(starship.MGLT, out int mglt) ? mglt : null,
                Model = starship.Model,
                Name = starship.Name,
                Passengers = int.TryParse(starship.Passengers, out int passengers) ? passengers : null,
                StarshipClass = starship.StarshipClass,
            };
        }

        private static Species ToSpecies(SwapiSpecies species)
        {
            return new Species()
            {
                Name = species.Name,
                Classification = species.Classification,
                FilmIds = species.Films.Select(film => int.Parse(film.Split("/", StringSplitOptions.RemoveEmptyEntries).Last())).ToList(),
            };
        }
        #endregion

        public async Task<IEnumerable<Person>> GetPeopleByNameAsync(string name, CancellationToken cancellationToken)
        {
            var swapiResults = await this.GetAllPagedResults<SwapiPerson>("people/?search=" + HttpUtility.UrlEncode(name), cancellationToken);
            return swapiResults.Select(person => ToPerson(person));
        }

        public async Task<IEnumerable<Planet>> GetPlanetsAsync(CancellationToken cancellationToken)
        {
            var swapiResults = await this.GetAllPagedResults<SwapiPlanet>("planets", cancellationToken);
            return swapiResults.Select(planet => ToPlanet(planet));
        }

        public async Task<IEnumerable<Species>> GetSpecies(CancellationToken cancellationToken)
        {
            var swapiResults = await this.GetAllPagedResults<SwapiSpecies>("species", cancellationToken);
            return swapiResults.Select(species => ToSpecies(species));
        }

        public async Task<IEnumerable<Starship>> GetStarshipsForPerson(Person person, CancellationToken cancellationToken)
        {
            // TODO: Instead of iterating over each starship in turn, we could request the data in parallel
            // in batches
            var client = this.GetClient();
            var result = new List<Starship>();
            foreach (var starshipRef in person.StarshipReferences)
            {
                var swapiStarship = await client.GetFromJsonAsync<SwapiStarship>(starshipRef, cancellationToken);
                result.Add(ToStarship(swapiStarship));
            }
            return result;
        }

        /// <summary>
        /// If the SWAPI data are paged, loop over all pages and aggregate the results
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<List<T>> GetAllPagedResults<T>(string url, CancellationToken cancellationToken)
        {
            List<T> results = new List<T>();
            string currentUrl = url;
            var client = this.GetClient();
            do
            {
                var result = await client.GetFromJsonAsync<SwapiPagedResult<T>>(currentUrl, cancellationToken);
                results.AddRange(result.Results);
                currentUrl = result.Next;
            } while (!string.IsNullOrWhiteSpace(currentUrl));

            return results;
        }
    }
}
