using SimplifiedSearch.SearchPipelines;
using SimplifiedSearch.SearchPipelines.ResultSelectors;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplifiedSearch.Tests.Configurations;

public sealed class ResultSelectorTop1 : IResultSelector
{
    public Task<IList<T>> RunAsync<T>(IList<SimilarityRankItem<T>> rankedList)
    {
        var results = rankedList
            .Where(x => x.SimilarityRank > 0)
            .OrderByDescending(x => x.SimilarityRank)
            .Take(1)
            .Select(x => x.Item)
            .ToArray();

        return Task.FromResult((IList<T>)results);
    }
}
