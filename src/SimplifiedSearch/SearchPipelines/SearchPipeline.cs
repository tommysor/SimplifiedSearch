using System;
using System.Collections.Generic;
using SimplifiedSearch.SearchPipelines.ResultSelectors;
using SimplifiedSearch.SearchPipelines.SimilarityRankPipelines;

namespace SimplifiedSearch.SearchPipelines;

internal class SearchPipeline : ISearchPipeline
{
    private readonly ISimilarityRankPipeline _similarityRankPipeline;
    private readonly IResultSelector _resultSelector;

    internal SearchPipeline(ISimilarityRankPipeline similarityRankPipeline, IResultSelector resultSelector)
    {
        _similarityRankPipeline = similarityRankPipeline ?? throw new ArgumentNullException(nameof(similarityRankPipeline));
        _resultSelector = resultSelector;
    }

    public IList<T> Search<T>(IList<T> list, string searchTerm, Func<T, string?> fieldToSearch)
    {
        if (list is null)
            throw new ArgumentNullException(nameof(list));
        if (string.IsNullOrEmpty(searchTerm))
            throw new ArgumentException($"{nameof(searchTerm)} must not be null or empty.", nameof(searchTerm));
        if (fieldToSearch is null)
            throw new ArgumentNullException(nameof(fieldToSearch));

        var listWithRank = _similarityRankPipeline.Run(list, searchTerm, fieldToSearch);
        var results = _resultSelector.Run(listWithRank);
        return results;
    }
}
