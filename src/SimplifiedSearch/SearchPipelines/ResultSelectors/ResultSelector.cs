using System.Collections.Generic;
using System.Linq;

namespace SimplifiedSearch.SearchPipelines.ResultSelectors;

internal sealed class ResultSelector : IResultSelector
{
    public IList<T> Run<T>(IList<SimilarityRankItem<T>> rankedList)
    {
        var results = rankedList
            .Where(x => x.SimilarityRank > 0)
            .OrderByDescending(x => x.SimilarityRank)
            .Select(x => x.Item)
            .ToArray();

        return results;
    }
}
