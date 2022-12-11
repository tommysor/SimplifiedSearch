using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplifiedSearch.SearchPipelines.SimilarityRankPipelines;

namespace SimplifiedSearch.SearchPipelines.ResultSelectors
{
    internal sealed class ResultSelector : IResultSelector
    {
        public Task<IList<T>> RunAsync<T>(IList<SimilarityRankItem<T>> rankedList)
        {
            var results = rankedList
                .Where(x => x.SimilarityRank > 0)
                .OrderByDescending(x => x.SimilarityRank)
                .Select(x => x.Item)
                .ToArray();

            return Task.FromResult((IList<T>)results);
        }
    }
}
