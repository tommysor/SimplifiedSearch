[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
See [Acknowledgements](#Acknowledgements) for additional license information covering parts of the project.
# SimplifiedSearch
Simple way to add ranked fuzzy matching search.\
For when you have up to a few thousand products, locations or similar and want to add a search that most users will see as smart, with minimal work.
## Intended use case
Searching through lists of short phrases like country names or the subject line in emails.\
Data in databases must first be loaded into memory in order to be searched.
## .NET support
Tested with: NETCOREAPP3.1, NET5.0, NET6.0
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
## Acknowledgements
### https://github.com/apache/lucenenet
Lucenenet is the main inspiration for SimplifiedSearch.\
SimplifiedSearch was started with the goal of delivering similar results to a spesific setup of Lucene analyzer and query.
### https://github.com/ninjanye/SearchExtensions
SearchExtensions is another inspiration for SimplifiedSearch.\
SimplifiedSearch delivers a simpler (and less configurable) experience.
### https://github.com/DanHarltey/Fastenshtein
Provides the distance calculation needed for fuzzy search.\
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg) https://github.com/DanHarltey/Fastenshtein/blob/master/LICENSE](https://github.com/DanHarltey/Fastenshtein/blob/master/LICENSE).
### https://github.com/thecoderok/Unidecode.NET
Provides the ascii folding needed to match accented characters to their ascii approximate equivalent (â, å, à, á, ä ≈ a).\
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg) https://github.com/thecoderok/Unidecode.NET/blob/master/LICENSE](https://github.com/thecoderok/Unidecode.NET/blob/master/LICENSE).
### https://github.com/annexare/Countries
For test data `tests/data/annexare/Countries/*`.\
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg) tests/data/annexare/Countries/LICENSE](tests/data/annexare/Countries/LICENSE).
### https://github.com/CivilServiceUSA/us-states
For test data `tests/data/CivilServiceUSA/us-states/*`.\
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg) tests/data/CivilServiceUSA/us-states/LICENSE](tests/data/CivilServiceUSA/us-states/LICENSE).
### https://github.com/linanqiu/reddit-dataset
For test data `tests/data/linanqiu/reddit-dataset/*`.\
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg) tests/data/linanqiu/reddit-dataset/README.md](tests/data/linanqiu/reddit-dataset/README.md) (se bottom of readme).
## Contributing
Bug reports, feature requests and pull requests are welcome.
- The focus of the project is in making the simple use case work well, not on supporting many special cases.
- For significant changes, make an issue for discussion before putting significant work into the change.
- Follow the established code format.
