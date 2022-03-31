using NUnit.Framework;
using StarWars.Exceptions;
using StarWars.Interfaces;
using StarWars.Models;
using StarWars.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StarWarsTests
{
    /// <summary>
    /// Unit tests for the StandardStarWarsCalculationService.  
    /// ensure 
    /// </summary>
    public class StandardStarWarsCalculationServiceTests
    {
        private IStarWarsCalculationService _dataService;

        [SetUp]
        public void Setup()
        {
            _dataService = new StandardStarWarsCalculationService(new StubSwapiService());
        }

        [Test]
        public async Task TestGetTotalPopulation()
        {
            var totalPopulation = await _dataService.GetTotalPopulation(CancellationToken.None);
            Assert.AreEqual(3000, totalPopulation);
        }

        [TestCase(1, "1,2")]
        [TestCase(2, "1,3")]
        [TestCase(3, "1,2,3")]
        public async Task GetSpeciesClassificationByMovie(int movieId, string classifications)
        {
            // Get all the classifications for the specified movie
            IEnumerable<SpeciesClassification> results = await _dataService.GetSpeciesClassificationByMovie(movieId, CancellationToken.None);
            // Get the list of classifications and concatenate them into a string for comparison with the inputted string
            var classificationResult = string.Join(",", results.Select(x => x.Classification).OrderBy(x => x));
            Assert.AreEqual(classifications, classificationResult);
        }

        /// <summary>
        /// Test the error handling for GetSpeciesClassificationByMovie if an invalid movieId is passed
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns></returns>
        [TestCase(0)]
        public async Task GetSpeciesClassificationByMovieInvalidId(int movieId)
        {
            try
            {
                IEnumerable<SpeciesClassification> results = await _dataService.GetSpeciesClassificationByMovie(movieId, CancellationToken.None);
                Assert.Fail("Was expecting a ResourceNotFoundException. Obtained no exception");
            }
            catch (ResourceNotFoundException)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail($"Was expecting a ResourceNotFoundException. Obtained exception of type {ex.GetType().AssemblyQualifiedName}");
            }
        }

        [TestCase(Constants.LukeSkywalker, "1,2,3")]
        [TestCase(Constants.TestUser, "")]
        public async Task TestGetStarshipsByPilot(string pilotName, string starshipNames)
        {
            var results = await _dataService.GetStarshipsForPilot(pilotName, CancellationToken.None);
            // Concatenate the names of all the starships into a list
            var allNames = string.Join(",", results.Select(x => x.Name).OrderBy(x => x));
            Assert.AreEqual(starshipNames, allNames);
        }
    }
}