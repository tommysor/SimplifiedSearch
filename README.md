# SimplifiedSearch
Simple way to add ranked fuzzy matching search.
For when you have up to a few thousand products, locations or similar and want to add a search that most users will see as smart, with minimal work.
## Intended use case
Searching through lists of short phrases like country names or the subject line in emails.
## .NET support
Tested with: NETCore3.1, NET5.0, NET6.0
## Quickstart
### Install
TODO
### Code
Use extension method `.SimplifiedSearchAsync(searchTerm, fieldToSearchLambda)`.
`fieldToSearchLambda` is optional. When missing, all fields will be searched (or the value, if the value is `string`, `Enum`, `int`, etc).
```c#
IList<Country> countries = GetListOfCountries();
IList<Country> matches = await countries.SimplifiedSearchAsync("tailand", x => x.CountryName);
foreach (var country in matches)
{
    Console.WriteLine($"# {country.CountryName}");
}
// Prints:
// # Thailand
// # Taiwan
```
