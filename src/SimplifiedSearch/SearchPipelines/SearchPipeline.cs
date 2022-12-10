using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplifiedSearch.SearchPipelines.TokenPipelines;

namespace SimplifiedSearch.SearchPipelines
{
    internal class SearchPipeline : ISearchPipeline
    {
        private readonly ITokenPipeline _tokenPipeline;

        internal SearchPipeline(ITokenPipeline tokenPipeline)
        {
            _tokenPipeline = tokenPipeline ?? throw new ArgumentNullException(nameof(tokenPipeline));
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
            var searchTermTokens = await _tokenPipeline.RunAsync(searchTerm).ConfigureAwait(false);

            var listWithRank = new List<(T, double)>();
            foreach (var item in list)
            {
                var fieldValue = fieldToSearch(item);
                if (fieldValue is null)
                    continue;
                if (fieldValue == "")
                    continue;
                var fieldValueTokens = await _tokenPipeline.RunAsync(fieldValue).ConfigureAwait(false);
                var similarityRank = await GetSimilarityRank(fieldValueTokens, searchTermTokens).ConfigureAwait(false);
                if (similarityRank > 0)
                    listWithRank.Add((item, similarityRank));
            }

            var results = listWithRank
                .OrderByDescending(x => x.Item2)
                .Select(x => x.Item1)
                .ToArray();

            return results;
        }

        private Task<double> GetSimilarityRank(string[] fieldValueTokens, string[] searchTermTokens)
        {
            var similarityRank = 0.0;
            foreach(var fieldValue in fieldValueTokens)
                foreach(var searchTerm in searchTermTokens)
                {
                    var similarityRankForToken = GetSimilarityRankForToken(fieldValue, searchTerm);
                    similarityRank += similarityRankForToken;
                }

            return Task.FromResult(similarityRank);
        }

        private static double GetSimilarityRankForToken(string fieldValue, string searchTerm)
        {
            var rank = GetSimilarityRankForTokenShort(fieldValue, searchTerm);
            
            if (searchTerm.Length > 1)
                rank += GetSimilarityRankForTokenFuzzy(fieldValue, searchTerm);

            return rank;
        }

        private static double GetSimilarityRankForTokenShort(string fieldValue, string searchTerm)
        {
            const int maxExactCheckLength = 5;
            var minLength = fieldValue.Length < searchTerm.Length ? fieldValue.Length : searchTerm.Length;
            if (maxExactCheckLength < minLength)
                minLength = maxExactCheckLength;

            var rank = 0.0;
            for (var i = 0; i < minLength; i++)
            {
                if (char.ToUpperInvariant(fieldValue[i]) == char.ToUpperInvariant(searchTerm[i]))
                {
                    rank += 0.1;
                }
                else
                {
                    break;
                }
            }

            return rank;
        }

        private static double GetSimilarityRankForTokenFuzzy(string fieldValue, string searchTerm)
        {
            // Shorten fieldValue to match start of word.
            // Add char to get better match when searchTerm is missing a character.
            var maxLength = searchTerm.Length + 1;
            if (fieldValue.Length > maxLength)
                fieldValue = fieldValue.Substring(0, maxLength);

            var distance = Fastenshtein.Levenshtein.Distance(fieldValue, searchTerm);
            switch (distance)
            {
                case 0:
                    return 5;
                case 1:
                    return 3;
                case 2:
                    return 1;
            }

            return 0;
        }
    }
}
