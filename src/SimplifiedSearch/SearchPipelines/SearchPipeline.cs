using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedSearch.SearchPipelines
{
    internal class SearchPipeline : ISearchPipeline
    {
        private readonly ISearchPipelineComponent[] _pipelineComponents;

        internal SearchPipeline(params ISearchPipelineComponent[] pipelineComponenets)
        {
            _pipelineComponents = pipelineComponenets ?? throw new ArgumentNullException(nameof(pipelineComponenets));
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
            var searchTermTokens = await GetSearchTermTokens(searchTerm).ConfigureAwait(false);

            var listWithRank = new List<(T, int)>();
            foreach (var item in list)
            {
                var fieldValue = fieldToSearch(item);
                if (fieldValue is null)
                    continue;
                if (fieldValue == "")
                    continue;
                var fieldValueTokens = await GetFieldTokens(fieldValue).ConfigureAwait(false);
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

        private async Task<string[]> GetSearchTermTokens(string searchTerm)
        {
            var searchTermTokens = new[] { searchTerm };
            foreach (var component in _pipelineComponents)
            {
                searchTermTokens = await component.RunAsync(searchTermTokens).ConfigureAwait(false);
            }

            return searchTermTokens;
        }

        private async Task<string[]> GetFieldTokens(string fieldValue)
        {
            // Project intended to search short texts.
            // If fed very long field values, only search first part of field.
            // In order to complete in reasonable time.
            if (fieldValue.Length > 5000)
#if NETSTANDARD2_0
                fieldValue = fieldValue.Substring(0, 5000);
#else
                fieldValue = fieldValue[0..5000];
#endif


            var fieldValueTokens = new[] { fieldValue };
            foreach (var component in _pipelineComponents)
            {
                fieldValueTokens = await component.RunAsync(fieldValueTokens).ConfigureAwait(false);
            }

            return fieldValueTokens;
        }

        private Task<int> GetSimilarityRank(string[] fieldValueTokens, string[] searchTermTokens)
        {
            var similarityRank = 0;
            foreach(var fieldValue in fieldValueTokens)
                foreach(var searchTerm in searchTermTokens)
                {
                    var similarityRankForToken = GetSimilarityRankForToken(fieldValue, searchTerm);
                    similarityRank += similarityRankForToken;
                }

            return Task.FromResult(similarityRank);
        }

        private static int GetSimilarityRankForToken(string fieldValue, string searchTerm)
        {
            // For very short SearchTerm, use exact match.
            if (searchTerm.Length <= 1)
            {
                if (fieldValue.Length >= 1)
                {
                    var fieldValueFirstChar = fieldValue.Substring(0, 1);
                    return string.Equals(fieldValueFirstChar, searchTerm, StringComparison.CurrentCultureIgnoreCase) ? 1 : 0;
                }
                else
                {
                    return 0;
                }
            }

            return GetSimilarityRankForTokenFuzzy(fieldValue, searchTerm);
        }

        private static int GetSimilarityRankForTokenFuzzy(string fieldValue, string searchTerm)
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
