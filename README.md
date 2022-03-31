# Star Wars Project
This is a simple demo project demonstrating how to build a RESTful API to process data taken from a third party 
system,  in this case the [Star Wars API (SWAPI)](https://swapi.dev/). 

## Design overview
The project has been implemented as an ASP.NET WebAPI service (using .NET 6.0) with the following design considerations:
- The WebAPI controller uses custom services injected through Dependency Injection (DI)
	- All services implement an interface that is used for DI
	- All services also reference each other through the DI interface
- Since there are only three endpoints, I've put them all into a single "StarWarsData" controller. If the number of requested endpoints
increases I could split it into multiple controllers
- I use one service to pull data from SWAPI and another service to perform calculations on it.
	- If the SWAPI interface changes, I only need to change the service that pulls the data. So long as I do not change the corresponding interface, I will not need to update the rest of the code.
	- I can easily unit test the calculation service by mocking the data-pulling service.
- No authentication is required to access this service.
	- As an enhancement, I could set up an Identity Server instance and use middleware to validate that a valid JWT token is provided in an Authentication header for each API request.
- NUnit unit tests are included as part of the solution; these are examples and not intended to provide 100% coverage.

## The API definition
The application has been deployed into Azure, and detailed Swagger documentation is available at 
[https://starwarsdemo123.azurewebsites.net/swagger/index.html](https://starwarsdemo123.azurewebsites.net/swagger/index.html).

We here summarize the different endpoints that are available:

### Starships
```
curl -X GET https://starwarsdemo123.azurewebsites.net/StarWarsData/default/starships
```
This returns information about the default starships (those associated with Luke Skywalker). If you want to query for another pilot, you can
use a different endpoint:
```
curl -X GET https://starwarsdemo123.azurewebsites.net/StarWarsData/pilots/Han%20Solo/starships
```
This returns information about all the starships related to a named pilot (in this case Han Solo). If the pilot isn't found or the name search
returns more than one pilot you'll get an error.

### Species classifications
```
curl -X GET https://starwarsdemo123.azurewebsites.net/StarWarsData/default/species/classifications
```
This returns the default species classifications (that is, the classification of all species in Episode 1). If you're interested in another movie 
you can use another endpoint:
```
curl -X GET https://starwarsdemo123.azurewebsites.net/StarWarsData/movies/4/species/classifications
```
This returns the classification of all species in episode 4.

### Planetary population
```
curl -X GET https://starwarsdemo123.azurewebsites.net/StarWarsData/planets/population
```
This returns the total population of all known planets with known populations.