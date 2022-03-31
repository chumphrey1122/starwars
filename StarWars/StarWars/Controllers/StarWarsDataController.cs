using Microsoft.AspNetCore.Mvc;
using StarWars.Exceptions;
using StarWars.Interfaces;
using StarWars.Models;

namespace StarWars.Controllers
{
    /// <summary>
    /// The default controller that contains all of the StarWarsData endpoints. Since there are only a handful of 
    /// endpoints, I'm including all of them in one controller. We might want to move endpoints to different
    /// controllers (e.g. a controller for starships, one for species) if the number of related endpoints starts increasing.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class StarWarsDataController : ControllerBase
    {
        private IStarWarsCalculationService _dataService;
        private ILogger<StarWarsDataController> _logger;
        public StarWarsDataController(IStarWarsCalculationService dataService, ILogger<StarWarsDataController> logger)
        {
            _dataService = dataService;
            _logger = logger;
        }

        /// <summary>
        /// Get all default starships (those associated with Luke Skywalker)
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(IEnumerable<Starship>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("default/starships")]
        public Task<IActionResult> GetStarshipsByPilot(CancellationToken cancellationToken)
        {
            return this.GetStarshipsByPilot("Luke Skywalker", cancellationToken);
        }

        /// <summary>
        /// Get all starships associated with the given pilot
        /// </summary>
        /// <param name="pilot"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(IEnumerable<Starship>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("pilots/{pilot}/starships")]
        public async Task<IActionResult> GetStarshipsByPilot([FromRoute] string pilot, CancellationToken cancellationToken)
        {
            try
            {
                return this.Ok(await _dataService.GetStarshipsForPilot(pilot, cancellationToken));
            }
            catch (ResourceNotFoundException)
            {
                _logger.LogError($"The specified pilot ({pilot}) could not be found");
                return this.NotFound();
            }
            catch (SearchReturnedMultipleResultsException)
            {
                _logger.LogError($"The specified pilot ({pilot}) matched more than one result");
                return this.BadRequest("The specified pilot matched more than one result");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem getting the data");
                return this.Problem("There was a problem getting your data");
            }
        }

        /// <summary>
        /// Get the total population of all known planets. If the population of a planet is unknown it will be
        /// excluded from this calculation.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("planets/population")]
        public async Task<IActionResult> GetPlanetaryPopulation(CancellationToken cancellationToken)
        {
            try
            {
                return this.Ok(await _dataService.GetTotalPopulation(cancellationToken));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem getting the data");
                return this.Problem("There was a problem getting your data");
            }
        }

        /// <summary>
        /// Get the default species classifications, i.e. those associated with Episode 1.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(IEnumerable<SpeciesClassification>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("default/species/classifications")]
        public Task<IActionResult> GetSpeciesClassification(CancellationToken cancellationToken)
        {
            return this.GetSpeciesClassification(1, cancellationToken);
        }

        /// <summary>
        /// Get the species classification for a specific movie.
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(IEnumerable<SpeciesClassification>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("movies/{movieId}/species/classifications")]
        public async Task<IActionResult> GetSpeciesClassification([FromRoute] int movieId, CancellationToken cancellationToken)
        {
            try
            {
                return this.Ok(await _dataService.GetSpeciesClassificationByMovie(movieId, cancellationToken));
            }
            catch (ResourceNotFoundException)
            {
                _logger.LogError($"The requested movie ({movieId}) was not found");
                return this.NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem getting the data");
                return this.Problem("There was a problem getting your data");
            }
        }


    }
}
