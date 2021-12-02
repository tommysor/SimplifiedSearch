using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedSearch.SearchPipelines
{
    internal class SearchPipeline
    {
        private readonly ISearchPipelineComponent[] _pipelineComponents;

        internal SearchPipeline(params ISearchPipelineComponent[] pipelineComponenets)
        {
            _pipelineComponents = pipelineComponenets ?? throw new ArgumentNullException(nameof(pipelineComponenets));
        }

        internal async Task<IList<T>> SearchAsync<T>(IList<T> list, string searchTerm, Func<T, string?> fieldToSearch)
        {
            if (list is null)
                throw new ArgumentNullException(nameof(list));
            if (string.IsNullOrEmpty(searchTerm))
                throw new ArgumentException($"{nameof(searchTerm)} must not be null or empty.", nameof(searchTerm));
            if (fieldToSearch is null)
                throw new ArgumentNullException(nameof(fieldToSearch));

            var searchTermTokens = await GetSearchTermTokens(searchTerm).ConfigureAwait(false);

            var listWithRank = new List<(T, int)>();
            foreach (var item in list)
            {
                var fieldValue = fieldToSearch(item);
                if (string.IsNullOrEmpty(fieldValue))
                    continue;
                if (fieldValue is null)
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
                fieldValue = fieldValue.Substring(0, 5000);
            
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
                    var fieldValue2 = fieldValue;

                    // Shorten fieldValue to match start of word.
                    // Add char to get better match when searchTerm is missing a character.
                    var maxLength = searchTerm.Length + 1;
                    if (fieldValue2.Length > maxLength)
                        fieldValue2 = fieldValue2.Substring(0, maxLength);

                    // Use fuzzy matching for longer words.
                    // For short words, use exact matching.
                    if (searchTerm.Length > 3)
                    {
                        var distance = Fastenshtein.Levenshtein.Distance(fieldValue2, searchTerm);
                        switch (distance)
                        {
                            case 0:
                                similarityRank += 5;
                                break;
                            case 1:
                                similarityRank += 3;
                                break;
                            case 2:
                                similarityRank += 1;
                                break;
                        }
                    }
                    else
                    {
                        if (searchTerm == fieldValue2)
                            similarityRank += 1;
                    }
                }

            return Task.FromResult(similarityRank);
        }
    }
}
