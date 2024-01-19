using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedSearch.SearchPipelines.SimilarityRankPipelines.Components
{
    internal sealed class MainSimilarityRanker : ISimilarityRankPipelineComponent
    {
        public Task<double> RunAsync(string[] fieldValueTokens, string[] searchTermTokens)
        {
            var similarityRank = 0.0;
            foreach(var fieldValue in fieldValueTokens)
                foreach(var searchTerm in searchTermTokens)
                {
                    if (searchTerm.Length > 1)
                    {
                        var similarityRankForToken = GetSimilarityRankForToken(fieldValue, searchTerm);
                        similarityRank += similarityRankForToken;
                    }
                }

            return Task.FromResult(similarityRank);
        }

        private static double GetSimilarityRankForToken(string fieldValue, string searchTerm)
        {
            // Shorten fieldValue to match start of word.
            // Add char to get better match when searchTerm is missing a character.
            double truncatedCharCount = 0;
            var maxLength = searchTerm.Length + 1;
            if (fieldValue.Length > maxLength)
            {
                truncatedCharCount = fieldValue.Length - maxLength;
                fieldValue = fieldValue.Substring(0, maxLength);
            }

            var distance = Fastenshtein.Levenshtein.Distance(fieldValue, searchTerm);
            double distanceScore = distance switch
            {
                0 => 5,
                1 => 3,
                2 => 1,
                _ => 0,
            };
            distanceScore -= truncatedCharCount / 10;
            if (distanceScore < 0)
            {
                distanceScore = 0;
            }
            return distanceScore;
        }
    }
}
