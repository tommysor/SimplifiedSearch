[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
# SimplifiedSearch
Simple way to add ranked fuzzy matching search.\
For when you have up to a few thousand products, locations or similar and want to add a search that most users will see as smart, with minimal work.
## Intended use case
Searching through lists of short phrases like country names or the subject line in emails.
## .NET support
Tested with: NET4.8, NETCOREAPP3.1, NET5.0, NET6.0
## Quickstart
### Install
`PM> Install-Package SimplifiedSearch`
### Code
Use extension method `.SimplifiedSearchAsync(searchTerm, propertyToSearchLambda)`.\
`propertyToSearchLambda` is optional. When missing, all properties will be searched (or the value, if the value is `string`, `Enum`, `int`, etc).
```csharp
using SimplifiedSearch;

IList<Country> countries = GetListOfCountries();
IList<Country> matches = await countries.SimplifiedSearchAsync("tailand", x => x.CountryName);
foreach (var country in matches)
{
    Console.WriteLine($"# {country.CountryName}");
}
// output:
// # Thailand
// # Taiwan
```
### Acknowledgements
#### https://github.com/annexare/Countries
For test data.
