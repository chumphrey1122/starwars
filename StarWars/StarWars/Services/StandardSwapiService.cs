using StarWars.Interfaces;
using StarWars.Models;
using System.Web;

namespace StarWars.Services
{
    public class StandardSwapiService : ISwapiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<StandardSwapiService> _logger;

        public StandardSwapiService(IHttpClientFactory httpClientFactory, ILogger<StandardSwapiService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public Task<List<Person>> GetPeopleByNameAsync(string name, CancellationToken cancellationToken)
        {
            return this.GetAllPagedResults<Person>("people/?search=" + HttpUtility.UrlEncode(name), cancellationToken);
        }

        public Task<List<Planet>> GetPlanetsAsync(CancellationToken cancellationToken)
        {
            return this.GetAllPagedResults<Planet>("planets", cancellationToken);
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
            var client = _httpClientFactory.CreateClient("swapi");
            do
            {
                var result = await client.GetFromJsonAsync<PagedResult<T>>(currentUrl, cancellationToken);
                results.AddRange(result.Results);
                if (string.IsNullOrWhiteSpace(result.Next))
                    break;
                currentUrl = result.Next;
            } while (true);

            return results;
        }
    }
}
