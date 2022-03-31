using StarWars.Exceptions;
using StarWars.Interfaces;
using StarWars.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StarWarsTests
{
    /// <summary>
    /// Stub implementation of ISwapiService that returns dummy data that is suitable for unit testing.
    /// </summary>
    internal class StubSwapiService : ISwapiService
    {
        public Task<IEnumerable<Person>> GetPeopleByNameAsync(string name, CancellationToken cancellationToken)
        {
            List<Person> result;
            switch (name)
            {
                case Constants.LukeSkywalker:
                case Constants.TestUser:
                    result = new List<Person>()
                    {
                        new Person() { Name = name },
                    };
                    break;
                case Constants.Skywalker:
                    throw new SearchReturnedMultipleResultsException();
                default:
                    throw new ResourceNotFoundException();
                    
            }
            return Task.FromResult<IEnumerable<Person>>(result);
        }

        public Task<IEnumerable<Planet>> GetPlanetsAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<IEnumerable<Planet>>(new List<Planet>()
            {
                new Planet() { Name = "1", Population = null},
                new Planet() {Name = "2", Population = 1000},
                new Planet() {Name = "3", Population = 2000},
            });
        }

        public Task<IEnumerable<Species>> GetSpecies(CancellationToken cancellationToken)
        {
            return Task.FromResult<IEnumerable<Species>>(new List<Species>() 
            { 
                new Species() {Name = "1", Classification = "1", FilmIds = new List<int>() {1, 2, 3}},
                new Species() {Name = "2", Classification = "2", FilmIds = new List<int>() {1, 3}},
                new Species() {Name = "3", Classification = "3", FilmIds = new List<int>() {2, 3}},
            });
        }

        public Task<IEnumerable<Starship>> GetStarshipsForPerson(Person person, CancellationToken cancellationToken)
        {
            List<Starship> result;
            switch (person.Name)
            {
                case Constants.LukeSkywalker:
                    result = new List<Starship>() 
                    { 
                        new Starship() {Name = "1"},
                        new Starship() {Name = "2"},
                        new Starship() {Name= "3"},
                    };
                    break;
                default:
                    result = new List<Starship>();
                    break;
            }
            return Task.FromResult<IEnumerable<Starship>>(result);
        }
    }
}
