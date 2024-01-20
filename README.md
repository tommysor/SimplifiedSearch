[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=tommysor_SimplifiedSearch&metric=alert_status)](https://sonarcloud.io/summary/overall?id=tommysor_SimplifiedSearch)
# SimplifiedSearch
Simple way to add ranked fuzzy matching search.\
For when you have up to a few thousand products, locations or similar and want to add a search that most users will see as smart, with minimal work.
## Intended use case
Searching through lists of short phrases like country names or the subject line in emails.\
Data in databases must first be loaded into memory in order to be searched.
## .NET support
Tested with: .NETFramework4.8, NET6.0, NET8.0
## Quickstart
### Install
[![Nuget](https://img.shields.io/nuget/v/SimplifiedSearch)](https://www.nuget.org/packages/SimplifiedSearch/)\
`> dotnet add package SimplifiedSearch`
### Code
Use extension method `.SimplifiedSearchAsync(searchTerm, propertyToSearchLambda)`.\
`propertyToSearchLambda` is optional. When missing, all properties will be searched (or the value, if the value is `string`, `Enum`, `int`, etc).
```csharp
using SimplifiedSearch;

IList<Country> countries = GetListOfCountries();
IList<Country> matches = await countries.SimplifiedSearchAsync("thaiwan", x => x.CountryName);
foreach (var country in matches)
{
    Console.WriteLine(country.CountryName);
}
// output:
// Taiwan
// Thailand
```
## Customization
New in version `1.3.0`.
```csharp
// Create searcher with custom selection of final result.
public class MyCustomSelector : SimplifiedSearch.SearchPipelines.ResultSelectors.IResultSelector
{
    public Task<IList<T>> RunAsync<T>(IList<SimilarityRankItem<T>> rankedList) => ...
}
SimplifiedSearchFactory.Instance.Add("MyCustomSearcher",
    c => c.ResultSelector = new MyCustomSelector());
var simplifiedSearch = SimplifiedSearchFactory.Instance.Create("MyCustomSearcher");
var searchResults = await simplifiedSearch.SimplifiedSearchAsync(list, "searchTerm");

// Override the default searcher, also used by the extension methods.
SimplifiedSearchFactory.Instance.Add(SimplifiedSearchFactory.DefaultName,
    c => c.ResultSelector = new MyCustomSelector());
var searchResults = await list.SimplifiedSearchAsync("searchTerm");
```
## Acknowledgements
### Inspiration
#### https://github.com/apache/lucenenet
Lucenenet is the main inspiration for SimplifiedSearch.\
SimplifiedSearch was originally started with the goal of delivering similar results to a spesific setup of Lucene analyzer and query.
### Enablers
#### https://github.com/DanHarltey/Fastenshtein
Provides the distance calculation needed for fuzzy search.\
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg) https://github.com/DanHarltey/Fastenshtein/blob/master/LICENSE](https://github.com/DanHarltey/Fastenshtein/blob/master/LICENSE).
#### https://github.com/thecoderok/Unidecode.NET
Provides the ascii folding needed to match accented characters to their ascii approximate equivalent (â, å, à, á, ä ≈ a).\
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg) https://github.com/thecoderok/Unidecode.NET/blob/master/LICENSE](https://github.com/thecoderok/Unidecode.NET/blob/master/LICENSE).
## Contributing
Bug reports, feature requests and pull requests are welcome.
- The focus of the project is in making the simple use case work well, not on supporting many special cases.
- For significant changes, make an issue for discussion before putting significant work into the change.
- Follow the established code format.
