using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplifiedSearch.SearchPipelines.SimilarityRankPipelines;

namespace SimplifiedSearch.SearchPipelines
{
    internal class SearchPipeline : ISearchPipeline
    {
        private readonly ISimilarityRankPipeline _similarityRankPipeline;

        internal SearchPipeline(ISimilarityRankPipeline similarityRankPipeline)
        {
            _similarityRankPipeline = similarityRankPipeline ?? throw new ArgumentNullException(nameof(similarityRankPipeline));
        }

        public Task<IList<T>> SearchAsync<T>(IList<T> list, string searchTerm, Func<T, string?> fieldToSearch)
        {
            if (list is null)
                throw new ArgumentNullException(nameof(list));
            if (string.IsNullOrEmpty(searchTerm))
                throw new ArgumentException($"{nameof(searchTerm)} must not be null or empty.", nameof(searchTerm));
            if (fieldToSearch is null)
                throw new ArgumentNullException(nameof(fieldToSearch));

            return SearchAsync2(list, searchTerm, fieldToSearch);
        }

        private async Task<IList<T>> SearchAsync2<T>(IList<T> list, string searchTerm, Func<T, string?> fieldToSearch)
        {
            var listWithRank = await _similarityRankPipeline.RunAsync(list, searchTerm, fieldToSearch);

            var results = listWithRank
                .Where(x => x.SimilarityRank > 0)
                .OrderByDescending(x => x.SimilarityRank)
                .Select(x => x.Item)
                .ToArray();

            return results;
        }
    }
}
