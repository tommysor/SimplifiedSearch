using SimplifiedSearch.SearchPipelines;
using SimplifiedSearch.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimplifiedSearch;

internal class SimplifiedSearchImpl : ISimplifiedSearch
{
    private readonly ISearchPipeline _searchPipeline;
    private readonly IPropertyBuilder _propertyBuilder;

    internal SimplifiedSearchImpl(ISearchPipeline searchPipeline, IPropertyBuilder propertyBuilder)
    {
        _searchPipeline = searchPipeline ?? throw new ArgumentNullException(nameof(searchPipeline));
        _propertyBuilder = propertyBuilder ?? throw new ArgumentNullException(nameof(propertyBuilder));
    }

    private IList<T> ExecuteSearch<T>(IList<T> searchThisList, string searchTerm, Func<T, string?>? propertyToSearchLambda)
    {
        // Validate arguments.
        if (searchThisList is null)
            throw new ArgumentNullException(nameof(searchThisList));

        // If nothing will be filtered. Do the fast thing, return the input list.
        if (string.IsNullOrEmpty(searchTerm))
            return searchThisList;

        // If no field is specified, build field of all properties.
        propertyToSearchLambda ??= _propertyBuilder.BuildPropertyToSearchLambda<T>();

        // Build the results.
        var results = _searchPipeline.Search(searchThisList, searchTerm, propertyToSearchLambda);

        return results;
    }

    public async Task<IList<T>> SimplifiedSearchAsync<T>(IList<T> searchThisList, string searchTerm, Func<T, string?>? propertyToSearchLambda)
    {
        // Make sure we actually get off the UI thread when used in desktop apps.
        return await Task.Run(() => ExecuteSearch(searchThisList, searchTerm, propertyToSearchLambda)).ConfigureAwait(false);
    }

    public Task<IList<T>> SimplifiedSearchAsync<T>(IList<T> searchThisList, string searchTerm)
    {
        return SimplifiedSearchAsync(searchThisList, searchTerm, null);
    }
}
