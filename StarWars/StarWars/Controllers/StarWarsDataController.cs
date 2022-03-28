using Microsoft.AspNetCore.Mvc;
using StarWars.Interfaces;

namespace StarWars.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StarWarsDataController : ControllerBase
    {
        private ISwapiService _swapiService;
        private ILogger<StarWarsDataController> _logger;
        public StarWarsDataController(ISwapiService swapiService, ILogger<StarWarsDataController> logger)
        {
            _swapiService = swapiService;
            _logger = logger;
        }

        [HttpGet("data")]
        public async Task<IActionResult> GetData(CancellationToken cancellationToken)
        {
            try
            {
                var res = await _swapiService.GetPeopleByNameAsync("Luke Skywalker", cancellationToken);
                var p = await _swapiService.GetPlanetsAsync(cancellationToken);
                return this.Ok(p);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem getting the data");
                return this.Problem("There was a problem getting your data");
            }
        }
    }
}
